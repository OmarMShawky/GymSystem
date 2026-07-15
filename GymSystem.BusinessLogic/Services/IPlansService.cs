using GymSystem.DataAccess.Entities;

namespace GymSystem.BusinessLogic.Services;

public interface IPlansService
{
    Task<Plan?> GetPlanByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Plan>> PlansAsync(CancellationToken cancellationToken);
}