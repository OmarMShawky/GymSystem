namespace GymSystem.BusinessLogic.Services;

public class PlansService : IPlansService
{
    private readonly IGenericRepository<Plan> _plansRepository;

    public PlansService(IGenericRepository<Plan> plansRepository)
    {
        _plansRepository = plansRepository;
    }

    public async Task<IEnumerable<Plan>> PlansAsync(CancellationToken cancellationToken)
    {
        return await _plansRepository.GetAllAsync(trackChanges: false, cancellationToken);
    }

    public async Task<Plan?> GetPlanByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _plansRepository.GetByIdAsync(id, cancellationToken);
    }
}
