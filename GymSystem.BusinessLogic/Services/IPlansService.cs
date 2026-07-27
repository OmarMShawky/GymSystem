namespace GymSystem.BusinessLogic.Services;

public interface IPlansService
{
    Task<Plan?> GetPlanByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Plan>> PlansAsync(CancellationToken cancellationToken);
    Task<EditPlanViewModel?> GetPlanForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> UpdatePlanAsync(int id, EditPlanViewModel editPlanViewModel, CancellationToken cancellationToken = default);
    Task<bool> TogglePlanStatusAsync(int id, CancellationToken cancellationToken = default);
}
