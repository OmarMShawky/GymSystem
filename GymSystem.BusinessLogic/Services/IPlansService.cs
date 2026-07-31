namespace GymSystem.BusinessLogic.Services;

public interface IPlansService
{

    Task<IEnumerable<Plan>> PlansAsync(CancellationToken cancellationToken);

    Task<Result<Plan>> GetPlanByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<EditPlanViewModel>> GetPlanForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> UpdatePlanAsync(int id, EditPlanViewModel editPlanViewModel, CancellationToken cancellationToken = default);
    Task<Result> TogglePlanStatusAsync(int id, CancellationToken cancellationToken = default);
}
