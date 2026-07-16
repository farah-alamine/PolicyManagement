using PolicyManagement.Core.Entities.Common;

namespace PolicyManagement.Core.Entities
{
    public class PolicyType : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IEnumerable<Policy> Policies { get; set; } = new List<Policy>();
    }
}
