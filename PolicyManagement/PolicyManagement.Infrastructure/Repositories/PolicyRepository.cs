using Microsoft.EntityFrameworkCore;
using PolicyManagement.Core.Entities;
using PolicyManagement.Core.Interfaces.Repositories;
using PolicyManagement.Infrastructure.Persistence;

namespace PolicyManagement.Infrastructure.Repositories
{
    public class PolicyRepository
    : GenericRepository<Policy>, IPolicyRepository
    {
        public PolicyRepository(PolicyManagementDbContext context) : base(context)
        {
        }

        public async Task<(IReadOnlyList<Policy> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = Context.Policies
                .AsNoTracking()
                .Include(x => x.PolicyType)
                .OrderByDescending(x => x.CreatedDate);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<Policy?> GetDetailsByRecordGuidAsync(
            Guid recordGuid,
            CancellationToken cancellationToken = default)
        {
            return await Context.Policies
                .AsNoTracking()
                .Include(x => x.PolicyType)
                .Include(x => x.Members)
                .Include(x => x.Claims)
                .FirstOrDefaultAsync(
                    x => x.RecordGuid == recordGuid,
                    cancellationToken);
        }
    }
}
