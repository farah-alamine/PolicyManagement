namespace PolicyManagement.Core.Models.Responses.PolicyTypes
{
    public class PolicyTypeResponse
    {
        public Guid RecordGuid { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
