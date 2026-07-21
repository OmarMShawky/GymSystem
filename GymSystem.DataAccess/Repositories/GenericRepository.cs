using GymSystem.DataAccess.Contracts;
using GymSystem.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly GymDbContext _context;

    public GenericRepository(GymDbContext context)
    {
        _context = context;
    }

    //Get All Members
    public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
        => trackChanges ? await _context.Set<TEntity>().ToListAsync(cancellationToken)
         : await _context.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);

    //Get Member By Id
    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);


    public async Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Add(entity);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Update(entity);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Remove(entity);
        return await _context.SaveChangesAsync();
    }
}
