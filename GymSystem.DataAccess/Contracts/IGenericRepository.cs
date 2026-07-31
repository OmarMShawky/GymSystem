using System.Linq.Expressions;

namespace GymSystem.DataAccess.Contracts;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    void Add(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity, CancellationToken cancellationToken = default);
    void Delete(TEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(
        Expression<Func<TEntity, object>>[] includes,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdWithIncludesAsync(
        int id,
        Expression<Func<TEntity, object>>[] includes,
        CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

}
