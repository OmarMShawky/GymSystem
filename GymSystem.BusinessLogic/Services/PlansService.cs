using GymSystem.DataAccess.Repositories;
using GymSystem.DataAccess.Data;
using GymSystem.DataAccess.Entities;
namespace GymSystem.BusinessLogic.Services;

public class PlansService : IPlansService
{
    private readonly IPlansRepository _plansRepository;

    public PlansService(IPlansRepository plansRepository)
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
