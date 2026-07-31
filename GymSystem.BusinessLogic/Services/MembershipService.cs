using AutoMapper;
using GymSystem.BusinessLogic.ViewModels.Memberships;
using System.Linq.Expressions;

namespace GymSystem.BusinessLogic.Services;

public class MembershipService(IUnitOfWork unitOfWork, IMapper mapper) : IMembershipService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    private static readonly Expression<Func<Membership, object>>[] _includes =
    [
        m => m.Member,
        m => m.Plan
    ];

    public async Task<IEnumerable<MembershipViewModel>> GetMembershipsAsync(CancellationToken cancellationToken = default)
    {
        var memberships = await _unitOfWork.GetRepository<Membership>()
            .GetAllWithIncludesAsync(_includes, cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<MembershipViewModel>>(
            memberships.OrderByDescending(m => m.EndDate));
    }

    public async Task<CreateMembershipViewModel> LoadLookupsAsync(
        CreateMembershipViewModel createMembershipViewModel, CancellationToken cancellationToken = default)
    {
        var members = await _unitOfWork.GetRepository<Member>()
            .GetAllAsync(cancellationToken: cancellationToken);

        var plans = await _unitOfWork.GetRepository<Plan>()
            .GetAllAsync(cancellationToken: cancellationToken);

        createMembershipViewModel.Members = members
            .OrderBy(m => m.Name)
            .Select(m => new LookupItemViewModel { Id = m.Id, Name = $"{m.Name} ({m.Email})" })
            .ToList();

        createMembershipViewModel.Plans = plans
            .Where(p => p.IsActive)
            .OrderBy(p => p.DurationDays)
            .Select(p => new LookupItemViewModel
            {
                Id = p.Id,
                Name = $"{p.Name} - {p.Price:0} EGP / {p.DurationDays} day(s)"
            })
            .ToList();

        return createMembershipViewModel;
    }

    public async Task<Result> CreateMembershipAsync(
        CreateMembershipViewModel createMembershipViewModel, CancellationToken cancellationToken = default)
    {

        var memberId = createMembershipViewModel.MemberId!.Value;
        var planId = createMembershipViewModel.PlanId!.Value;

        var member = await _unitOfWork.GetRepository<Member>()
            .GetByIdAsync(memberId, cancellationToken);

        if (member is null)
            return Result.NotFound("The selected member no longer exists.");

        var plan = await _unitOfWork.GetRepository<Plan>()
            .GetByIdAsync(planId, cancellationToken);

        if (plan is null)
            return Result.NotFound("The selected plan no longer exists.");

        if (!plan.IsActive)
            return Result.Fail("That plan is inactive and cannot be sold.");

        var membershipRepo = _unitOfWork.GetRepository<Membership>();

        var today = DateOnly.FromDateTime(DateTime.Now);

        var alreadySubscribed = await membershipRepo
            .AnyAsync(m => m.MemberId == memberId && m.EndDate > today, cancellationToken);

        if (alreadySubscribed)
            return Result.Conflict("This member already has an active membership.");

        membershipRepo.Add(new Membership
        {
            MemberId = memberId,
            PlanId = planId,

            EndDate = today.AddDays(plan.DurationDays)
        }, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0
            ? Result.Ok()
            : Result.Fail("The membership could not be created.");
    }

    public async Task<Result<MembershipDetailsViewModel>> GetMembershipDetailsAsync(
        int id, CancellationToken cancellationToken = default)
    {
        var membership = await _unitOfWork.GetRepository<Membership>()
            .GetByIdWithIncludesAsync(id, _includes, cancellationToken);

        return membership is null
            ? Result.NotFound<MembershipDetailsViewModel>("Membership not found.")
            : _mapper.Map<MembershipDetailsViewModel>(membership);
    }

    public async Task<Result> DeleteMembershipAsync(int id, CancellationToken cancellationToken = default)
    {
        var membershipRepo = _unitOfWork.GetRepository<Membership>();

        var membership = await membershipRepo.GetByIdAsync(id, cancellationToken);

        if (membership is null)
            return Result.NotFound("Membership not found.");

        membership.IsDeleted = true;

        membershipRepo.Update(membership, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0
            ? Result.Ok()
            : Result.Fail("The membership could not be deleted.");
    }
}
