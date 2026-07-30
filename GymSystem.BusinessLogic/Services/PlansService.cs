using AutoMapper;

namespace GymSystem.BusinessLogic.Services;

public class PlansService(IUnitOfWork unitOfWork, IMapper mapper) : IPlansService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<Plan>> PlansAsync(CancellationToken cancellationToken)
    {
        return await _unitOfWork.GetRepository<Plan>()
            .GetAllAsync(trackChanges: false, cancellationToken);
    }

    public async Task<Plan?> GetPlanByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _unitOfWork.GetRepository<Plan>()
            .GetByIdAsync(id, cancellationToken);
    }

    public async Task<EditPlanViewModel?> GetPlanForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        var plan = await _unitOfWork.GetRepository<Plan>()
            .GetByIdAsync(id, cancellationToken);

        if (plan is null)
            return null;

        return _mapper.Map<EditPlanViewModel>(plan);
    }

    public async Task<bool> UpdatePlanAsync(int id, EditPlanViewModel editPlanViewModel, CancellationToken cancellationToken = default)
    {
        var planRepo = _unitOfWork.GetRepository<Plan>();

        var plan = await planRepo.GetByIdAsync(id, cancellationToken);

        if (plan is null)
            return false;

        if (await HasActiveMembershipsAsync(id, cancellationToken))
            return false;

        _mapper.Map(editPlanViewModel, plan);

        planRepo.Update(plan, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0;
    }

    public async Task<bool> TogglePlanStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var planRepo = _unitOfWork.GetRepository<Plan>();

        var plan = await planRepo.GetByIdAsync(id, cancellationToken);

        if (plan is null)
            return false;

        // An active plan can only be switched off once nobody is still subscribed to it.
        if (plan.IsActive && await HasActiveMembershipsAsync(id, cancellationToken))
            return false;

        plan.IsActive = !plan.IsActive;

        planRepo.Update(plan, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0;
    }

    private async Task<bool> HasActiveMembershipsAsync(int planId, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        return await _unitOfWork.GetRepository<Membership>()
            .AnyAsync(m => m.PlanId == planId && m.EndDate > today, cancellationToken);
    }
}
