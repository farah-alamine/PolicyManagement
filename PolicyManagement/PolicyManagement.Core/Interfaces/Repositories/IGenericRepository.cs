using PolicyManagement.Core.Entities.Common;
using System.Linq.Expressions;

namespace PolicyManagement.Core.Interfaces.IRepositories
{
    public interface IGenericRepository<T>
     where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<T?> GetByRecordGuidAsync(Guid recordGuid, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        void Update(T entity);

        void Delete(T entity);
    }
}
