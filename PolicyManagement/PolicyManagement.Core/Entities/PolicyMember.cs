using PolicyManagement.Core.Entities.Common;

namespace PolicyManagement.Core.Entities
{
    public class PolicyMember : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string RelationshipToPolicyHolder { get; set; } = string.Empty;
        public int PolicyId { get; set; }


        #region Virtual
        public virtual Policy Policy { get; set; } = null!;
        #endregion
    }
}
