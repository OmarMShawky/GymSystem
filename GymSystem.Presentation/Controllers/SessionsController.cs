using GymSystem.BusinessLogic.Common;
using GymSystem.BusinessLogic.Services;
using GymSystem.BusinessLogic.ViewModels.Sessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Presentation.Controllers;

[Authorize]
public class SessionsController(ISessionService sessionService) : Controller
{
    private readonly ISessionService _sessionService = sessionService;

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Message = TempData["Hello from ViewBag"] as string;
        var sessions = await _sessionService.GetSessionsAsync(cancellationToken);
        return View(sessions);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var createSessionViewModel = await _sessionService
            .LoadLookupsAsync(new CreateSessionViewModel(), cancellationToken);

        return View(createSessionViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSessionViewModel createSessionViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(await _sessionService.LoadLookupsAsync(createSessionViewModel, cancellationToken));

        var result = await _sessionService.CreateSessionAsync(createSessionViewModel, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["Hello from ViewBag"] = "Session created successfully!";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result.Error!);

        return View(await _sessionService.LoadLookupsAsync(createSessionViewModel, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var result = await _sessionService.GetSessionDetailsAsync(id, cancellationToken);

        if (result.IsFailure)
            return this.FromFailure(result);

        return View(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var result = await _sessionService.GetSessionForEditAsync(id, cancellationToken);

        if (result.IsFailure)
            return this.FromFailure(result);

        if (result.Value.Status != SessionStatus.Upcoming)
        {
            TempData["Hello from ViewBag"] = $"An {result.Value.Status} session cannot be edited.";
            return RedirectToAction(nameof(Index));
        }

        return View(result.Value);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditSessionViewModel editSessionViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(await _sessionService.LoadLookupsAsync(editSessionViewModel, cancellationToken));

        var result = await _sessionService.UpdateSessionAsync(id, editSessionViewModel, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["Hello from ViewBag"] = "Session updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        if (result.Status == ResultStatus.NotFound)
            return NotFound();

        ModelState.AddModelError(string.Empty, result.Error!);

        return View(await _sessionService.LoadLookupsAsync(editSessionViewModel, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _sessionService.GetSessionDetailsAsync(id, cancellationToken);

        if (result.IsFailure)
            return this.FromFailure(result);

        ViewBag.Message = TempData["Hello from ViewBag"] as string;

        return View(result.Value);
    }

    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var result = await _sessionService.DeleteSessionAsync(id, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["Hello from ViewBag"] = "Session deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        if (result.Status == ResultStatus.NotFound)
            return NotFound();

        TempData["Hello from ViewBag"] = result.Error;
        return RedirectToAction(nameof(Delete), new { id });
    }
}
