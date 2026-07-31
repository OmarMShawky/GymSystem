using GymSystem.BusinessLogic.ViewModels.Dashboard;

namespace GymSystem.BusinessLogic.Services;

public interface IDashboardService
{
    Task<DashboardViewModel> GetDashboardAsync(CancellationToken cancellationToken = default);
}
