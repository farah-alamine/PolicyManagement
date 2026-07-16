using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyManagement.Core.Models.Requests.Policies
{
    public class UpdatePolicyRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public Guid PolicyTypeGuid { get; set; }
    }
}
