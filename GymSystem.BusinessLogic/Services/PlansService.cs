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

    public async Task<EditPlanViewModel?> GetPlanForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        var plan = await _plansRepository.GetByIdAsync(id, cancellationToken);

        if (plan is null)
            return null;

        return new EditPlanViewModel
        {
            Id = plan.Id,
            Name = plan.Name,
            Description = plan.Description,
            DurationDays = plan.DurationDays,
            Price = plan.Price,
            IsActive = plan.IsActive
        };
    }

    public async Task<bool> UpdatePlanAsync(int id, EditPlanViewModel editPlanViewModel, CancellationToken cancellationToken = default)
    {
        var plan = await _plansRepository.GetByIdAsync(id, cancellationToken);

        if (plan is null)
            return false;

        plan.Name = editPlanViewModel.Name;
        plan.Description = editPlanViewModel.Description;
        plan.DurationDays = editPlanViewModel.DurationDays;
        plan.Price = editPlanViewModel.Price;
        plan.IsActive = editPlanViewModel.IsActive;

        return (await _plansRepository.UpdateAsync(plan, cancellationToken)) > 0;
    }

    public async Task<bool> TogglePlanStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var plan = await _plansRepository.GetByIdAsync(id, cancellationToken);

        if (plan is null)
            return false;

        // Plans are never hard deleted; activation state is toggled instead so
        // historical memberships keep pointing at a valid plan.
        plan.IsActive = !plan.IsActive;

        return (await _plansRepository.UpdateAsync(plan, cancellationToken)) > 0;
    }
}
