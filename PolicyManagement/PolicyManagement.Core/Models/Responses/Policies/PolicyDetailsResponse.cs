using PolicyManagement.Core.Models.DTOs;

namespace PolicyManagement.Core.Models.Responses.Policies
{
    public class PolicyDetailsResponse
    {
        public Guid RecordGuid { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public Guid PolicyTypeGuid { get; set; }

        public string PolicyTypeName { get; set; } = string.Empty;

        public DateTimeOffset CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTimeOffset? LastUpdatedDate { get; set; }

        public string? LastUpdatedBy { get; set; }

        public IReadOnlyList<PolicyMemberDto> Members { get; set; } = new List<PolicyMemberDto>();

        public IReadOnlyList<ClaimDto> Claims { get; set; } = new List<ClaimDto>();
    }
}
