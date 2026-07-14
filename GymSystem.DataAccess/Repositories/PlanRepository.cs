using GymSystem.DataAccess.Data;
using GymSystem.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.DataAccess.Repositories;

public class PlansRepository : IPlansRepository
{
    private readonly GymDbContext _context;

    public PlansRepository(GymDbContext context)
    {
        _context = context;
    }

    //Get All Plans
    public async Task<IEnumerable<Plan>> GetAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
        => trackChanges ? await _context.Plans.ToListAsync(cancellationToken)
         : await _context.Plans.AsNoTracking().ToListAsync(cancellationToken);

    //Get Plan By Id
    public async Task<Plan?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Plans.FindAsync([id], cancellationToken);


    public async Task AddAsync(Plan plan, CancellationToken cancellationToken = default)
    {
        _context.Plans.Add(plan);
        await SaveChangesAsync();
    }

    public void Update(Plan plan, CancellationToken cancellationToken = default)
        => _context.Update(plan);

    public void Delete(Plan plan, CancellationToken cancellationToken = default)
        => _context.Remove(plan);

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

}
