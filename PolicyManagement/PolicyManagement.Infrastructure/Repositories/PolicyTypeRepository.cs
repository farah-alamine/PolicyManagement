using PolicyManagement.Core.Entities;
using PolicyManagement.Core.Interfaces.Repositories;
using PolicyManagement.Infrastructure.Persistence;

namespace PolicyManagement.Infrastructure.Repositories
{
    public class PolicyTypeRepository
    : GenericRepository<PolicyType>, IPolicyTypeRepository
    {
        public PolicyTypeRepository(PolicyManagementDbContext context) : base(context)
        {
        }

    }
}
