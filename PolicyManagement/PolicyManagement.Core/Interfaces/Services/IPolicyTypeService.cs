using PolicyManagement.Core.Models.Requests.PolicyTypes;
using PolicyManagement.Core.Models.Responses.PolicyTypes;

namespace PolicyManagement.Core.Interfaces.Services
{
    public interface IPolicyTypeService
    {
        Task<IEnumerable<PolicyTypeResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<PolicyTypeResponse> CreateAsync(CreatePolicyTypeRequest request, CancellationToken cancellationToken = default);
    }
}
