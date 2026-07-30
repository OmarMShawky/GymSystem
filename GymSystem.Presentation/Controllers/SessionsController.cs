using GymSystem.BusinessLogic.Services;
using GymSystem.BusinessLogic.ViewModels.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Presentation.Controllers;

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

        if (result is CreateSessionResult.Success)
        {
            TempData["Hello from ViewBag"] = "Session created successfully!";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, DescribeFailure(result));

        return View(await _sessionService.LoadLookupsAsync(createSessionViewModel, cancellationToken));
    }

    private static string DescribeFailure(CreateSessionResult result) => result switch
    {
        CreateSessionResult.CategoryNotFound => "The selected category no longer exists.",
        CreateSessionResult.TrainerNotFound => "The selected trainer no longer exists.",
        CreateSessionResult.SpecialtyMismatch => "The selected trainer's specialty does not match the session category.",
        CreateSessionResult.TrainerBusy => "The trainer already has a session scheduled in that time slot.",
        _ => "The session could not be created."
    };
}
