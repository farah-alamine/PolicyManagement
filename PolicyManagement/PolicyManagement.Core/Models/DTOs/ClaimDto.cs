using PolicyManagement.Core.Models.Enums;

namespace PolicyManagement.Core.Models.DTOs
{
    public class ClaimDto
    {
        public Guid RecordGuid { get; set; }

        public string ClaimNumber { get; set; } = string.Empty;

        public DateTime ClaimDate { get; set; }

        public decimal Amount { get; set; }

        public ClaimStatus Status { get; set; }

        public string? Description { get; set; }
    }
}
