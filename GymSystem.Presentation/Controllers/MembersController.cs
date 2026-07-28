using GymSystem.BusinessLogic.Services;
using GymSystem.BusinessLogic.ViewModels.Members;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
namespace GymSystem.Presentation.Controllers;

public class MembersController : Controller
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Message = TempData["Hello from ViewBag"] as string;
        var members = await _memberService.GetMembersAsync(cancellationToken);
        return View(members);
    }

    // create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMemberViewModel memberViewModel, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return View(memberViewModel);

        // call service to create member
        var result = await _memberService.CreateMemberAsync(memberViewModel, cancellationToken);
        if (result)
            TempData["Hello from ViewBag"] = "Member created successfully!";
        else
            TempData["Hello from ViewBag"] = "Member creation failed!";

        ModelState.AddModelError(string.Empty, "Email or Phone already exists.");

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var membershipDetails = await _memberService.GetMemberDetailsAsync(id);

        if (membershipDetails is null)
            return NotFound();

        return View(membershipDetails);
    }
    [HttpGet]
    public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken cancellationToken)
    {
        var healthRecordDetails = await _memberService.GetHealthRecordDetailsAsync(id, cancellationToken);

        if (healthRecordDetails is null)
            return NotFound();

        return View(healthRecordDetails);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var memberDetails = await _memberService.GetMemberDetailsForEditAsync(id, cancellationToken);
        if (memberDetails is null)
            return NotFound();
        return View(memberDetails);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditMemberViewModel editMemberViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(editMemberViewModel);

        // call service to update member
        var result = await _memberService.UpdateMemberAsync(id, editMemberViewModel, cancellationToken);
        if (!result)
        {
            ModelState.AddModelError(string.Empty, "Update failed. Email or phone may already be in use.");
            return View(editMemberViewModel);
        }

        TempData["Hello from ViewBag"] = "Member updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _memberService.DeleteMemberAsync(id, cancellationToken);

        TempData["Hello from ViewBag"] = result
            ? "Member deleted successfully!"
            : "Member delete failed!";

        return RedirectToAction(nameof(Index));
    }
}
