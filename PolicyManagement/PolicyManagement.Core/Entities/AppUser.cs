using PolicyManagement.Core.Entities.Common;

namespace PolicyManagement.Core.Entities
{
    public class AppUser : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public int TenantId { get; set; }

        public Tenant Tenant { get; set; } = null!;
    }
}
