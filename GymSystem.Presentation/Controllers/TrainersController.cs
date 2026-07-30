using GymSystem.BusinessLogic.Common;
using GymSystem.BusinessLogic.Services;
using GymSystem.BusinessLogic.ViewModels.Trainers;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Presentation.Controllers;

public class TrainersController(ITrainerService trainerService) : Controller
{
    private readonly ITrainerService _trainerService = trainerService;

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Message = TempData["Hello from ViewBag"] as string;
        var trainers = await _trainerService.GetTrainersAsync(cancellationToken);
        return View(trainers);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        return View(await _trainerService.LoadLookupsAsync(new CreateTrainerViewModel(), cancellationToken));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTrainerViewModel createTrainerViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(await _trainerService.LoadLookupsAsync(createTrainerViewModel, cancellationToken));

        var result = await _trainerService.CreateTrainerAsync(createTrainerViewModel, cancellationToken);

        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View(await _trainerService.LoadLookupsAsync(createTrainerViewModel, cancellationToken));
        }

        TempData["Hello from ViewBag"] = "Trainer created successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var result = await _trainerService.GetTrainerDetailsAsync(id, cancellationToken);

        if (result.IsFailure)
            return this.FromFailure(result);

        return View(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var result = await _trainerService.GetTrainerForEditAsync(id, cancellationToken);

        if (result.IsFailure)
            return this.FromFailure(result);

        return View(result.Value);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditTrainerViewModel editTrainerViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(await _trainerService.LoadLookupsAsync(editTrainerViewModel, cancellationToken));

        var result = await _trainerService.UpdateTrainerAsync(id, editTrainerViewModel, cancellationToken);

        if (result.Status == ResultStatus.NotFound)
            return NotFound();

        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View(await _trainerService.LoadLookupsAsync(editTrainerViewModel, cancellationToken));
        }

        TempData["Hello from ViewBag"] = "Trainer updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    // Step 1 of delete: show the confirmation page.
    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _trainerService.GetTrainerDetailsAsync(id, cancellationToken);

        if (result.IsFailure)
            return this.FromFailure(result);

        ViewBag.HasScheduledSessions = await _trainerService.HasScheduledSessionsAsync(id, cancellationToken);

        return View(result.Value);
    }

    // Step 2 of delete: the confirmed POST performs the permanent delete.
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var result = await _trainerService.DeleteTrainerAsync(id, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["Hello from ViewBag"] = "Trainer deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        if (result.Status == ResultStatus.NotFound)
            return NotFound();

        TempData["Hello from ViewBag"] = result.Error;
        return RedirectToAction(nameof(Delete), new { id });
    }
}
