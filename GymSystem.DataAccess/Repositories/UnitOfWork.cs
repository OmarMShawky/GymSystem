using GymSystem.DataAccess.Contracts;
using GymSystem.DataAccess.Data;
using System.Collections.Concurrent;

namespace GymSystem.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly GymDbContext _context;

    private readonly ConcurrentDictionary<string, object> _repositories = new();
    public IGenericRepository<Member> Member => GetRepository<Member>();

    public UnitOfWork(GymDbContext context, IGenericRepository<Member> memberRepository)
    {
        _context = context;
        _repositories[nameof(Member)] = memberRepository;
    }

    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
    {
        var entityName = typeof(TEntity).Name;

        if (_repositories.TryGetValue(entityName, out object? value))
            return (IGenericRepository<TEntity>)value;

        var repo = new GenericRepository<TEntity>(_context);
        _repositories.TryAdd(entityName, repo);
        return repo;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
