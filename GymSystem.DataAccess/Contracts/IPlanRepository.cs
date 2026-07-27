namespace GymSystem.DataAccess.Contracts;

public interface IPlansRepository
{
    Task<IEnumerable<Plan>> GetAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<Plan?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Plan plan, CancellationToken cancellationToken = default);
    Task UpdateAsync(Plan plan, CancellationToken cancellationToken = default);
    Task DeleteAsync(Plan plan, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync();
}
