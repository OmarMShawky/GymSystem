using GymSystem.DataAccess.Data;
using GymSystem.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Presentation.Controllers;

public class PlansController(IPlanRepository planRepository) : Controller
{
    private readonly IPlanRepository _planRepo = planRepository;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var plans = await _planRepo.GetAllAsync();
        return View(plans);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (id <= 0)
        {
            return RedirectToAction(nameof(Index));
        }

        var plan = _planRepo.GetByIdAsync(id);

        if (plan == null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(plan);
    }
}
