using PolicyManagement.Core.Models.Enums;

namespace PolicyManagement.Core.Models.Requests.Claims
{
    public class ClaimRequest
    {
        public string ClaimNumber { get; set; } = string.Empty;
        public DateTime ClaimDate { get; set; }
        public decimal Amount { get; set; }
        public ClaimStatus Status { get; set; } 
        public string? Description { get; set; }
    }
}
