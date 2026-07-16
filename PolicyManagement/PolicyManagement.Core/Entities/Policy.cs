using PolicyManagement.Core.Entities.Common;

namespace PolicyManagement.Core.Entities
{
    public class Policy : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int PolicyTypeId { get; set; }
        public IEnumerable<PolicyMember> Members { get; set; } = new List<PolicyMember>();
        public IEnumerable<Claim> Claims { get; set; } = new List<Claim>();

        #region Virtual
        public virtual PolicyType PolicyType { get; set; } = null!;
        #endregion
    }
}
