using PolicyManagement.Core.Entities.Common;

namespace PolicyManagement.Core.Entities
{
    public class Tenant : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Identifier { get; set; } = string.Empty;

        public string ConnectionString { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
    }
}
