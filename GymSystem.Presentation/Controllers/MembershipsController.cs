using GymSystem.BusinessLogic.Common;
using GymSystem.BusinessLogic.Services;
using GymSystem.BusinessLogic.ViewModels.Memberships;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Presentation.Controllers;

[Authorize]
public class MembershipsController(IMembershipService membershipService) : Controller
{
    private readonly IMembershipService _membershipService = membershipService;

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Message = TempData["Hello from ViewBag"] as string;
        var memberships = await _membershipService.GetMembershipsAsync(cancellationToken);
        return View(memberships);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var viewModel = await _membershipService
            .LoadLookupsAsync(new CreateMembershipViewModel(), cancellationToken);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMembershipViewModel createMembershipViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(await _membershipService.LoadLookupsAsync(createMembershipViewModel, cancellationToken));

        var result = await _membershipService.CreateMembershipAsync(createMembershipViewModel, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["Hello from ViewBag"] = "Membership created successfully!";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result.Error!);

        return View(await _membershipService.LoadLookupsAsync(createMembershipViewModel, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var result = await _membershipService.GetMembershipDetailsAsync(id, cancellationToken);

        if (result.IsFailure)
            return this.FromFailure(result);

        return View(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _membershipService.GetMembershipDetailsAsync(id, cancellationToken);

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
        var result = await _membershipService.DeleteMembershipAsync(id, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["Hello from ViewBag"] = "Membership deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        if (result.Status == ResultStatus.NotFound)
            return NotFound();

        TempData["Hello from ViewBag"] = result.Error;
        return RedirectToAction(nameof(Delete), new { id });
    }
}
