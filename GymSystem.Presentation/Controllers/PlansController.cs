using GymSystem.BusinessLogic.Common;
using GymSystem.BusinessLogic.Services;
using GymSystem.BusinessLogic.ViewModels.Plans;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Presentation.Controllers;

public class PlansController(IPlansService service) : Controller
{
    private readonly IPlansService _service = service;

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
            return RedirectToAction(nameof(Index));

        var result = await _service.GetPlanByIdAsync(id, cancellationToken);

        if (result.IsFailure)
            return RedirectToAction(nameof(Index));

        return View(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return RedirectToAction(nameof(Index));

        var result = await _service.GetPlanForEditAsync(id, cancellationToken);

        if (result.IsFailure)
            return this.FromFailure(result);

        return View(result.Value);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditPlanViewModel editPlanViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(editPlanViewModel);

        var result = await _service.UpdatePlanAsync(id, editPlanViewModel, cancellationToken);

        if (result.Status == ResultStatus.NotFound)
            return NotFound();

        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
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

        TempData["Hello from ViewBag"] = result.IsSuccess
            ? "Plan status updated successfully!"
            : result.Error;

        return RedirectToAction(nameof(Index));
    }
}
