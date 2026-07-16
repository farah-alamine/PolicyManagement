using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyManagement.Core.Models.DTOs
{
    public class PolicyMemberDto
    {
        public Guid RecordGuid { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string RelationshipToPolicyHolder { get; set; } = string.Empty;
    }
}
