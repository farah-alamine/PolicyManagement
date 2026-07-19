namespace PolicyManagement.Core.Models.Requests.PolicyMember
{
    public class PolicyMemberRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string RelationshipToPolicyHolder { get; set; } = string.Empty;
    }
}
