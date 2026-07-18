using PolicyManagement.Core.Models.Requests.Authentication;
using PolicyManagement.Core.Models.Responses.Authentication;

namespace PolicyManagement.Core.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<LoginResponse?> LoginAsync(
            LoginRequest request,
            CancellationToken cancellationToken = default);
    }
}
