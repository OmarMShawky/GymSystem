using GymSystem.DataAccess.Entities;

namespace GymSystem.DataAccess.Repositories;

public interface IPlansRepository
{
    Task<IEnumerable<Plan>> GetAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<Plan?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Plan plan, CancellationToken cancellationToken = default);
    void Update(Plan plan, CancellationToken cancellationToken = default);
    void Delete(Plan plan, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync();
}
