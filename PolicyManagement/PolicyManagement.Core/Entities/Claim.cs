using PolicyManagement.Core.Entities.Common;
using PolicyManagement.Core.Models.Enums;

namespace PolicyManagement.Core.Entities
{
    public class Claim : BaseEntity
    {
        public string ClaimNumber { get; set; } = string.Empty;
        public DateTime ClaimDate { get; set; }
        public decimal Amount { get; set; }
        public ClaimStatus Status { get; set; }
        public string? Description { get; set; }
        public int PolicyId { get; set; }

        #region Virtual
        public virtual Policy Policy { get; set; } = null!;
        #endregion
    }
}
