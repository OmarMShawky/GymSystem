using GymSystem.BusinessLogic.ViewModels.Dashboard;

namespace GymSystem.BusinessLogic.Services;

public class DashboardService(IUnitOfWork unitOfWork) : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DashboardViewModel> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;
        var today = DateOnly.FromDateTime(now);

        var memberRepo = _unitOfWork.GetRepository<Member>();
        var sessionRepo = _unitOfWork.GetRepository<Session>();

        var totalMembers = await memberRepo.CountAsync(cancellationToken: cancellationToken);

        var activeMemberIds = (await _unitOfWork.GetRepository<Membership>()
                .GetAllAsync(cancellationToken: cancellationToken))
            .Where(m => m.EndDate > today)
            .Select(m => m.MemberId)
            .Distinct()
            .Count();

        return new DashboardViewModel
        {
            TotalMembers = totalMembers,
            ActiveMembers = activeMemberIds,
            TotalTrainers = await _unitOfWork.GetRepository<Trainer>()
                .CountAsync(cancellationToken: cancellationToken),

            UpcomingSessions = await sessionRepo.CountAsync(s => s.StartDate > now, cancellationToken),
            OngoingSessions = await sessionRepo.CountAsync(s => s.StartDate <= now && s.EndDate >= now, cancellationToken),
            CompletedSessions = await sessionRepo.CountAsync(s => s.EndDate < now, cancellationToken),

            ActivePlans = await _unitOfWork.GetRepository<Plan>()
                .CountAsync(p => p.IsActive, cancellationToken)
        };
    }
}
