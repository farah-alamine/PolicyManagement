using Microsoft.EntityFrameworkCore;
using PolicyManagement.Core.Entities.Common;
using PolicyManagement.Core.Interfaces.IRepositories;
using PolicyManagement.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PolicyManagement.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly PolicyManagementDbContext Context;
        protected readonly DbSet<T> DbSet;

        public GenericRepository(PolicyManagementDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }

        public async Task<T?> GetByRecordGuidAsync(
            Guid recordGuid,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .FirstOrDefaultAsync(
                    x => x.RecordGuid == recordGuid,
                    cancellationToken);
        }

        public async Task<IReadOnlyList<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(
                predicate,
                cancellationToken);
        }

        public async Task AddAsync(
            T entity,
            CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }
    }
}
