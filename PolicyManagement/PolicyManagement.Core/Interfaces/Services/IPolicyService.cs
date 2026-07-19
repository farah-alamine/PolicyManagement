using PolicyManagement.Core.Models.Requests.Policies;
using PolicyManagement.Core.Models.Responses.Common;
using PolicyManagement.Core.Models.Responses.Policies;

namespace PolicyManagement.Core.Interfaces.Services
{
    public interface IPolicyService
    {
        Task<PagedResponse<PolicyResponse>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken = default);

        Task<PolicyDetailsResponse?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<PolicyDetailsResponse> CreateAsync(
            CreatePolicyRequest request,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(
            Guid id,
            UpdatePolicyRequest request,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
