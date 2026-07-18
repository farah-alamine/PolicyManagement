namespace PolicyManagement.Core.Models.Requests.PolicyTypes
{
    public class CreatePolicyTypeRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

    }
}
