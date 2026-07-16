namespace PolicyManagement.Core.Models.Responses.Policies
{
    public class PolicyResponse
    {
        public Guid RecordGuid { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string PolicyTypeName { get; set; } = string.Empty;

        public DateTimeOffset CreatedDate { get; set; }
    }
}
