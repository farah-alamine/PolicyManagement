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
            string? searchTerm,
            CancellationToken cancellationToken = default)
        {
            var query = Context.Policies
                .AsNoTracking()
                .Include(x => x.PolicyType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalizedSearchTerm = searchTerm.Trim().ToLower();

                query = query.Where(policy =>
                    policy.Name.ToLower().Contains(normalizedSearchTerm) ||
                    (
                        policy.Description != null &&
                        policy.Description
                            .ToLower()
                            .Contains(normalizedSearchTerm)
                    ) ||
                    policy.PolicyType.Name
                        .ToLower()
                        .Contains(normalizedSearchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedDate)
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
