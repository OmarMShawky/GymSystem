using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Contracts;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
