using GymSystem.BusinessLogic.Services;
using GymSystem.BusinessLogic.ViewModels.Trainers;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Presentation.Controllers;

public class TrainersController : Controller
{
    private readonly ITrainerService _trainerService;

    public TrainersController(ITrainerService trainerService)
    {
        _trainerService = trainerService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Message = TempData["Hello from ViewBag"] as string;
        var trainers = await _trainerService.GetTrainersAsync(cancellationToken);
        return View(trainers);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTrainerViewModel createTrainerViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(createTrainerViewModel);

        var result = await _trainerService.CreateTrainerAsync(createTrainerViewModel, cancellationToken);

        if (!result)
        {
            ModelState.AddModelError(string.Empty, "Email or phone already exists.");
            return View(createTrainerViewModel);
        }

        TempData["Hello from ViewBag"] = "Trainer created successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var trainer = await _trainerService.GetTrainerDetailsAsync(id, cancellationToken);

        if (trainer is null)
            return NotFound();

        return View(trainer);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var trainer = await _trainerService.GetTrainerForEditAsync(id, cancellationToken);

        if (trainer is null)
            return NotFound();

        return View(trainer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditTrainerViewModel editTrainerViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(editTrainerViewModel);

        var result = await _trainerService.UpdateTrainerAsync(id, editTrainerViewModel, cancellationToken);

        if (!result)
        {
            ModelState.AddModelError(string.Empty, "Update failed. Email or phone may already be in use.");
            return View(editTrainerViewModel);
        }

        TempData["Hello from ViewBag"] = "Trainer updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    // Step 1 of delete: show the confirmation page.
    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var trainer = await _trainerService.GetTrainerDetailsAsync(id, cancellationToken);

        if (trainer is null)
            return NotFound();

        ViewBag.HasScheduledSessions = await _trainerService.HasScheduledSessionsAsync(id, cancellationToken);

        return View(trainer);
    }

    // Step 2 of delete: the confirmed POST performs the permanent delete.
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var result = await _trainerService.DeleteTrainerAsync(id, cancellationToken);

        switch (result)
        {
            case DeleteTrainerResult.Success:
                TempData["Hello from ViewBag"] = "Trainer deleted successfully!";
                return RedirectToAction(nameof(Index));

            case DeleteTrainerResult.HasScheduledSessions:
                TempData["Hello from ViewBag"] = "This trainer has scheduled sessions and cannot be deleted.";
                return RedirectToAction(nameof(Delete), new { id });

            default:
                return NotFound();
        }
    }
}
