using PolicyManagement.Core.Models.Requests.Claims;
using PolicyManagement.Core.Models.Requests.PolicyMember;

namespace PolicyManagement.Core.Models.Requests.Policies
{
    public class CreatePolicyRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public Guid PolicyTypeGuid { get; set; }
       
        public List<PolicyMemberRequest> Members { get; set; } = [];

        public List<ClaimRequest> Claims { get; set; } = [];
    }
}
