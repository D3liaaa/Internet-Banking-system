using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace InternetBanking.Infrastructure.InfrastructureBase
{
    public interface IGenericRepository<T> where T : class
    {
        // Get all entities
        Task<IEnumerable<T>> GetAllAsync(CancellationToken ct);

        // Get an entity by its ID
        Task<T> GetByIdAsync(int id, CancellationToken ct);

        // Add a new entity
        Task AddAsync(T entity, CancellationToken ct);

        // Update an existing entity
        Task UpdateAsync(T entity, CancellationToken ct);

        // Delete an entity
        Task DeleteAsync(T entity, CancellationToken ct);

        Task DeleteRangeAsync(ICollection<T> entities);

        Task UpdateRangeAsync(ICollection<T> entities);

        Task SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        void Commit();
        void Rollback();
        IQueryable<T> GetTableNoTracking();

        IQueryable<T> GetTableAsTracking();

        Task AddRangeAsync(ICollection<T> entities);
        IQueryable<T> GetTableWithInclude(params Expression<Func<T, object>>[] includes);


    }
}
