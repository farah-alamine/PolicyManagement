using PolicyManagement.Core.Entities;

namespace PolicyManagement.Core.Interfaces.IRepositories
{
    public interface IPolicyRepository : IGenericRepository<Policy>
    {
        Task<(IReadOnlyList<Policy> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<Policy?> GetDetailsByRecordGuidAsync(
            Guid recordGuid,
            CancellationToken cancellationToken = default);
    }
}
