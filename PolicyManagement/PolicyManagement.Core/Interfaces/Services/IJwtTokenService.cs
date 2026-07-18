using PolicyManagement.Core.Entities;

namespace PolicyManagement.Core.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(
            AppUser user,
            DateTime expiresAt);
    }
}
