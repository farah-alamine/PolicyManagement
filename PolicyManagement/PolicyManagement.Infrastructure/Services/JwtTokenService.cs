using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PolicyManagement.Core.Entities;
using PolicyManagement.Core.Interfaces.Services;
using PolicyManagement.Core.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PolicyManagement.Infrastructure.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenService(
            IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(
            AppUser user,
            DateTime expiresAt)
        {
            var claims = new List<System.Security.Claims.Claim>
        {
            new(
                JwtRegisteredClaimNames.Sub,
                user.RecordGuid.ToString()),

            new(
                JwtRegisteredClaimNames.Email,
                user.Email),

            new(
                ClaimTypes.NameIdentifier,
                user.RecordGuid.ToString()),

            new(
                ClaimTypes.Name,
                user.FullName),

            new(
                ClaimTypes.Role,
                user.Role),
            new(
                "tenant_guid",
                user.Tenant.RecordGuid.ToString()),

            new(
                "tenant_identifier",
                user.Tenant.Identifier),
            new(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()),
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
