using PolicyManagement.Core.Entities;

namespace PolicyManagement.Core.Interfaces.Repositories
{
    public interface IPolicyRepository : IGenericRepository<Policy>
    {
        Task<(IReadOnlyList<Policy> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            CancellationToken cancellationToken = default);

        Task<Policy?> GetDetailsByRecordGuidAsync(
            Guid recordGuid,
            CancellationToken cancellationToken = default);
    }
}
