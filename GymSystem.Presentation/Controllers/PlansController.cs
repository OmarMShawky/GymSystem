using GymSystem.BusinessLogic.Services;
using GymSystem.BusinessLogic.ViewModels.Plans;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Presentation.Controllers;

public class PlansController : Controller
{
    private readonly IPlansService _service;

    public PlansController(IPlansService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Message = TempData["Hello from ViewBag"] as string;
        var plans = await _service.PlansAsync(cancellationToken);
        return View(plans);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return RedirectToAction(nameof(Index));
        }

        var plan = await _service.GetPlanByIdAsync(id, cancellationToken);

        if (plan == null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(plan);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return RedirectToAction(nameof(Index));
        }

        var plan = await _service.GetPlanForEditAsync(id, cancellationToken);

        if (plan is null)
        {
            return NotFound();
        }

        return View(plan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditPlanViewModel editPlanViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(editPlanViewModel);

        var result = await _service.UpdatePlanAsync(id, editPlanViewModel, cancellationToken);

        if (!result)
        {
            ModelState.AddModelError(string.Empty, "Update failed. The plan may no longer exist.");
            return View(editPlanViewModel);
        }

        TempData["Hello from ViewBag"] = "Plan updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id, CancellationToken cancellationToken)
    {
        var result = await _service.TogglePlanStatusAsync(id, cancellationToken);

        TempData["Hello from ViewBag"] = result
            ? "Plan status updated successfully!"
            : "Plan status update failed!";

        return RedirectToAction(nameof(Index));
    }
}
