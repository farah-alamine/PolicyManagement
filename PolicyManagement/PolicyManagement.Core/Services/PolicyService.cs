using PolicyManagement.Core.Entities;
using PolicyManagement.Core.Exceptions;
using PolicyManagement.Core.Interfaces.Repositories;
using PolicyManagement.Core.Interfaces.Services;
using PolicyManagement.Core.Models.DTOs;
using PolicyManagement.Core.Models.Requests.Policies;
using PolicyManagement.Core.Models.Responses.Common;
using PolicyManagement.Core.Models.Responses.Policies;
using PolicyManagement.Core.UnitOfWork;

namespace PolicyManagement.Core.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly IGenericRepository<PolicyType> _policyTypeRepository;
        private readonly IGenericRepository<PolicyMember> _policyMemberRepository;
        private readonly IGenericRepository<Claim> _claimRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PolicyService(
            IPolicyRepository policyRepository,
            IGenericRepository<PolicyType> policyTypeRepository,
            IGenericRepository<PolicyMember> policyMemberRepository,
            IGenericRepository<Claim> claimRepository,
            IUnitOfWork unitOfWork)
        {
            _policyRepository = policyRepository;
            _policyTypeRepository = policyTypeRepository;
            _policyMemberRepository = policyMemberRepository;
            _claimRepository = claimRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResponse<PolicyResponse>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            pageNumber = pageNumber < 1
                ? 1
                : pageNumber;

            pageSize = pageSize is < 1 or > 100
                ? 10
                : pageSize;

            var result = await _policyRepository.GetPagedAsync(
                pageNumber,
                pageSize,
                cancellationToken);

            return new PagedResponse<PolicyResponse>
            {
                Items = result.Items
                    .Select(MapToPolicyResponse)
                    .ToList(),

                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = result.TotalCount
            };
        }

        public async Task<PolicyDetailsResponse?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                return null;
            }

            var policy =
                await _policyRepository.GetDetailsByRecordGuidAsync(
                    id,
                    cancellationToken);

            return policy is null
                ? null
                : MapToPolicyDetailsResponse(policy);
        }

        public async Task<PolicyDetailsResponse> CreateAsync(
        CreatePolicyRequest request,
        CancellationToken cancellationToken = default)
        {
            var policyType =
                await _policyTypeRepository.GetByRecordGuidAsync(
                    request.PolicyTypeGuid,
                    cancellationToken);

            if (policyType is null)
            {
                throw new BadRequestException(
                    "The selected policy type does not exist.");
            }

            var policy = new Policy
            {
                Name = request.Name.Trim(),
                Description = NormalizeOptionalText(request.Description),
                EffectiveDate = request.EffectiveDate,
                ExpiryDate = request.ExpiryDate,
                PolicyTypeId = policyType.Id,

                Members = request.Members
                    .Select(member => new PolicyMember
                    {
                        FirstName = member.FirstName.Trim(),
                        LastName = member.LastName.Trim(),
                        DateOfBirth = member.DateOfBirth,
                        RelationshipToPolicyHolder =
                            member.RelationshipToPolicyHolder.Trim()
                    })
                    .ToList(),

                Claims = request.Claims
                    .Select(claim => new Claim
                    {
                        ClaimNumber = claim.ClaimNumber.Trim(),
                        ClaimDate = claim.ClaimDate,
                        Amount = claim.Amount,
                        Status = claim.Status,
                        Description = NormalizeOptionalText(claim.Description)
                    })
                    .ToList()
            };

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                await _policyRepository.AddAsync(
                    policy,
                    cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(
                    cancellationToken);

                throw;
            }

            var createdPolicy =
                await _policyRepository.GetDetailsByRecordGuidAsync(
                    policy.RecordGuid,
                    cancellationToken);

            if (createdPolicy is null)
            {
                throw new InvalidOperationException(
                    "The policy was created but could not be retrieved.");
            }

            return MapToPolicyDetailsResponse(createdPolicy);
        }

        public async Task<bool> UpdateAsync(
         Guid id,
         UpdatePolicyRequest request,
         CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                return false;
            }

            var policy =
                await _policyRepository.GetByRecordGuidAsync(
                    id,
                    cancellationToken);

            if (policy is null)
            {
                return false;
            }

            var policyType =
                await _policyTypeRepository.GetByRecordGuidAsync(
                    request.PolicyTypeGuid,
                    cancellationToken);

            if (policyType is null)
            {
                throw new BadRequestException(
                    "The selected policy type does not exist.");
            }

            policy.Name = request.Name.Trim();
            policy.Description = NormalizeOptionalText(request.Description);
            policy.EffectiveDate = request.EffectiveDate;
            policy.ExpiryDate = request.ExpiryDate;
            policy.PolicyTypeId = policyType.Id;

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var existingMembers =
                    await _policyMemberRepository.FindAsync(
                        member => member.PolicyId == policy.Id,
                        cancellationToken);

                foreach (var member in existingMembers)
                {
                    _policyMemberRepository.Delete(member);
                }

                var existingClaims =
                    await _claimRepository.FindAsync(
                        claim => claim.PolicyId == policy.Id,
                        cancellationToken);

                foreach (var claim in existingClaims)
                {
                    _claimRepository.Delete(claim);
                }

                foreach (var memberRequest in request.Members)
                {
                    var member = new PolicyMember
                    {
                        FirstName = memberRequest.FirstName.Trim(),
                        LastName = memberRequest.LastName.Trim(),
                        DateOfBirth = memberRequest.DateOfBirth,
                        RelationshipToPolicyHolder =
                            memberRequest.RelationshipToPolicyHolder.Trim(),
                        PolicyId = policy.Id
                    };

                    await _policyMemberRepository.AddAsync(
                        member,
                        cancellationToken);
                }

                foreach (var claimRequest in request.Claims)
                {
                    var claim = new Claim
                    {
                        ClaimNumber = claimRequest.ClaimNumber.Trim(),
                        ClaimDate = claimRequest.ClaimDate,
                        Amount = claimRequest.Amount,
                        Status = claimRequest.Status,
                        Description =
                            NormalizeOptionalText(claimRequest.Description),
                        PolicyId = policy.Id
                    };

                    await _claimRepository.AddAsync(
                        claim,
                        cancellationToken);
                }

                _policyRepository.Update(policy);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(
                    cancellationToken);

                throw;
            }
        }

        public async Task<bool> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                return false;
            }

            var policy =
                await _policyRepository.GetByRecordGuidAsync(
                    id,
                    cancellationToken);

            if (policy is null)
            {
                return false;
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                _policyRepository.Delete(policy);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(
                    cancellationToken);

                throw;
            }
        }


        #region Helper Methods
        private static string? NormalizeOptionalText(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }

        private static PolicyResponse MapToPolicyResponse(Policy policy)
        {
            return new PolicyResponse
            {
                RecordGuid = policy.RecordGuid,
                Name = policy.Name,
                Description = policy.Description,
                EffectiveDate = policy.EffectiveDate,
                ExpiryDate = policy.ExpiryDate,
                PolicyTypeName = policy.PolicyType.Name,
                CreatedDate = policy.CreatedDate
            };
        }

        private static PolicyDetailsResponse MapToPolicyDetailsResponse(Policy policy)
        {
            return new PolicyDetailsResponse
            {
                RecordGuid = policy.RecordGuid,
                Name = policy.Name,
                Description = policy.Description,
                EffectiveDate = policy.EffectiveDate,
                ExpiryDate = policy.ExpiryDate,
                PolicyTypeGuid = policy.PolicyType.RecordGuid,
                PolicyTypeName = policy.PolicyType.Name,
                CreatedDate = policy.CreatedDate,
                CreatedBy = policy.CreatedBy,
                LastUpdatedDate = policy.LastUpdatedDate,
                LastUpdatedBy = policy.LastUpdatedBy,

                Members = policy.Members
                    .Select(member => new PolicyMemberDto
                    {
                        RecordGuid = member.RecordGuid,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        DateOfBirth = member.DateOfBirth,
                        RelationshipToPolicyHolder = member.RelationshipToPolicyHolder
                    })
                    .ToList(),

                Claims = policy.Claims
                    .Select(claim => new ClaimDto
                    {
                        RecordGuid = claim.RecordGuid,
                        ClaimNumber = claim.ClaimNumber,
                        ClaimDate = claim.ClaimDate,
                        Amount = claim.Amount,
                        Status = claim.Status,
                        Description = claim.Description
                    })
                    .ToList()
            };
        }
        #endregion
    }
}
