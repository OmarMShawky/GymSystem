using GymSystem.DataAccess.Contracts;
using GymSystem.DataAccess.Data;
using System.Linq.Expressions;

namespace GymSystem.DataAccess.Repositories;

public class GenericRepository<TEntity>(GymDbContext context)
    : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly GymDbContext _context = context;

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
        => trackChanges ? await _context.Set<TEntity>().ToListAsync(cancellationToken)
         : await _context.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(
        Expression<Func<TEntity, object>>[] includes,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
        => await BuildQuery(includes, trackChanges).ToListAsync(cancellationToken);

    public async Task<TEntity?> GetByIdWithIncludesAsync(
        int id,
        Expression<Func<TEntity, object>>[] includes,
        CancellationToken cancellationToken = default)
        => await BuildQuery(includes, trackChanges: false)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private IQueryable<TEntity> BuildQuery(
        Expression<Func<TEntity, object>>[] includes, bool trackChanges)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (!trackChanges)
            query = query.AsNoTracking();

        foreach (var include in includes ?? [])
            query = query.Include(include);

        return query;
    }

    public void Add(TEntity entity, CancellationToken cancellationToken = default)
        => _context.Set<TEntity>().Add(entity);

    public void Update(TEntity entity, CancellationToken cancellationToken = default)
        => _context.Set<TEntity>().Update(entity);

    public void Delete(TEntity entity, CancellationToken cancellationToken = default)
        => _context.Set<TEntity>().Remove(entity);

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => _context.Set<TEntity>().AnyAsync(predicate, cancellationToken);

    public Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        => predicate is null
            ? _context.Set<TEntity>().CountAsync(cancellationToken)
            : _context.Set<TEntity>().CountAsync(predicate, cancellationToken);

    public Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => _context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
}
