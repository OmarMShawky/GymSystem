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

    public async Task<Result<Plan>> GetPlanByIdAsync(int id, CancellationToken cancellationToken)
    {
        var plan = await _unitOfWork.GetRepository<Plan>()
            .GetByIdAsync(id, cancellationToken);

        return plan is null
            ? Result.NotFound<Plan>("Plan not found.")
            : plan;
    }

    public async Task<Result<EditPlanViewModel>> GetPlanForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        var plan = await _unitOfWork.GetRepository<Plan>()
            .GetByIdAsync(id, cancellationToken);

        if (plan is null)
            return Result.NotFound<EditPlanViewModel>("Plan not found.");

        return _mapper.Map<EditPlanViewModel>(plan);
    }

    public async Task<Result> UpdatePlanAsync(int id, EditPlanViewModel editPlanViewModel, CancellationToken cancellationToken = default)
    {
        var planRepo = _unitOfWork.GetRepository<Plan>();

        var plan = await planRepo.GetByIdAsync(id, cancellationToken);

        if (plan is null)
            return Result.NotFound("Plan not found.");

        if (await HasActiveMembershipsAsync(id, cancellationToken))
            return Result.Conflict("This plan has active memberships and cannot be changed.");

        _mapper.Map(editPlanViewModel, plan);

        planRepo.Update(plan, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0
            ? Result.Ok()
            : Result.Fail("The plan could not be updated.");
    }

    public async Task<Result> TogglePlanStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var planRepo = _unitOfWork.GetRepository<Plan>();

        var plan = await planRepo.GetByIdAsync(id, cancellationToken);

        if (plan is null)
            return Result.NotFound("Plan not found.");

        // An active plan can only be switched off once nobody is still subscribed to it.
        if (plan.IsActive && await HasActiveMembershipsAsync(id, cancellationToken))
            return Result.Conflict("This plan has active memberships and cannot be deactivated.");

        plan.IsActive = !plan.IsActive;

        planRepo.Update(plan, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0
            ? Result.Ok()
            : Result.Fail("The plan status could not be updated.");
    }

    private async Task<bool> HasActiveMembershipsAsync(int planId, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        return await _unitOfWork.GetRepository<Membership>()
            .AnyAsync(m => m.PlanId == planId && m.EndDate > today, cancellationToken);
    }
}
