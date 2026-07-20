using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PolicyManagement.Core.Entities;
using PolicyManagement.Core.Interfaces.Services;
using PolicyManagement.Core.Models.Requests.Authentication;
using PolicyManagement.Core.Models.Responses.Authentication;
using PolicyManagement.Core.Settings;
using PolicyManagement.Infrastructure.Persistence;

namespace PolicyManagement.Infrastructure.Services
{
    public class AuthenticationService
        : IAuthenticationService
    {
        private readonly AuthenticationDbContext _dbContext;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(
            AuthenticationDbContext dbContext,
            IPasswordHasher<AppUser> passwordHasher,
            IJwtTokenService jwtTokenService,
            IOptions<JwtSettings> jwtSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginResponse?> LoginAsync(
            LoginRequest request,
            CancellationToken cancellationToken = default)
        {
            var normalizedEmail = request.Email
                .Trim()
                .ToLowerInvariant();

            var user = await _dbContext.AppUsers
          .Include(currentUser => currentUser.Tenant)
          .FirstOrDefaultAsync(
              currentUser =>
                  currentUser.Email.ToLower()
                      == normalizedEmail,
              cancellationToken);

            if (
            user is null ||
            !user.IsActive ||
            !user.Tenant.IsActive)
            {
                return null;
            }

            var verificationResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    request.Password);

            if (verificationResult
                == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var expiresAt = DateTime.UtcNow.AddMinutes(
                _jwtSettings.ExpiryMinutes);

            var token = _jwtTokenService.GenerateToken(
                user,
                expiresAt);

            return new LoginResponse
            {
                Token = token,
                ExpiresAt = expiresAt,
                UserGuid = user.RecordGuid,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}