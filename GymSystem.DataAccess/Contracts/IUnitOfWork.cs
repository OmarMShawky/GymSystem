namespace GymSystem.DataAccess.Contracts;

public interface IUnitOfWork
{
    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
