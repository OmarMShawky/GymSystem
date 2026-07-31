using GymSystem.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Presentation.Controllers;

[Authorize]
public class HomeController(IDashboardService dashboardService) : Controller
{
    private readonly IDashboardService _dashboardService = dashboardService;

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var dashboard = await _dashboardService.GetDashboardAsync(cancellationToken);

        return View(dashboard);
    }
}
