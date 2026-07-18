using PolicyManagement.Core.Entities;
using PolicyManagement.Core.Interfaces.Repositories;
using PolicyManagement.Core.Interfaces.Services;
using PolicyManagement.Core.Models.Requests.PolicyTypes;
using PolicyManagement.Core.Models.Responses.PolicyTypes;
using PolicyManagement.Core.UnitOfWork;

namespace PolicyManagement.Core.Services
{
    public class PolicyTypeService : IPolicyTypeService
    {
        private readonly IPolicyTypeRepository _policyTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PolicyTypeService(
            IPolicyTypeRepository policyTypeRepository,
            IUnitOfWork unitOfWork)
        {
            _policyTypeRepository = policyTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PolicyTypeResponse>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var policyTypes =
                await _policyTypeRepository.GetAllAsync(cancellationToken);

            return policyTypes
                .OrderBy(x => x.Name)
                .Select(x => new PolicyTypeResponse
                {
                    RecordGuid = x.RecordGuid,
                    Name = x.Name,
                    Description = x.Description
                });
        }

        public async Task<PolicyTypeResponse> CreateAsync(
            CreatePolicyTypeRequest request,
            CancellationToken cancellationToken = default)
        {
            var policyType = new PolicyType
            {
                Name = request.Name.Trim()
            };

            await _policyTypeRepository.AddAsync(
                policyType,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new PolicyTypeResponse
            {
                RecordGuid = policyType.RecordGuid,
                Name = policyType.Name
            };
        }
    }
}
