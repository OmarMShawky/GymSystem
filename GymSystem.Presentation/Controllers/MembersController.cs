using GymSystem.BusinessLogic.Common;
using GymSystem.BusinessLogic.Services;
using GymSystem.BusinessLogic.ViewModels.Members;
using GymSystem.DataAccess.Data.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Presentation.Controllers;

[Authorize(Roles = IdentitySeeder.SuperAdminRole)]
public class MembersController(IMemberService memberService, IFileService fileService) : Controller
{
    private readonly IMemberService _memberService = memberService;
    private readonly IFileService _fileService = fileService;

    [HttpGet]
    public IActionResult Picture(string fileName)
    {
        var result = _fileService.GetFile(FileSettings.MemberPhotosFolder, fileName);

        if (result.IsFailure)
            return this.FromFailure(result);

        return File(result.Value.Content, result.Value.ContentType);
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Message = TempData["Hello from ViewBag"] as string;
        var members = await _memberService.GetMembersAsync(cancellationToken);
        return View(members);
    }

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

        var result = await _memberService.CreateMemberAsync(memberViewModel, cancellationToken);

        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View(memberViewModel);
        }

        TempData["Hello from ViewBag"] = "Member created successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var result = await _memberService.GetMemberDetailsAsync(id, cancellationToken);

        if (result.IsFailure)
            return this.FromFailure(result);

        return View(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken cancellationToken)
    {
        var result = await _memberService.GetHealthRecordDetailsAsync(id, cancellationToken);

        if (result.IsFailure)
            return this.FromFailure(result);

        return View(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var result = await _memberService.GetMemberDetailsForEditAsync(id, cancellationToken);

        if (result.IsFailure)
            return this.FromFailure(result);

        return View(result.Value);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditMemberViewModel editMemberViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(editMemberViewModel);

        var result = await _memberService.UpdateMemberAsync(id, editMemberViewModel, cancellationToken);

        if (result.Status == ResultStatus.NotFound)
            return NotFound();

        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
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

        TempData["Hello from ViewBag"] = result.IsSuccess
            ? "Member deleted successfully!"
            : result.Error;

        return RedirectToAction(nameof(Index));
    }
}
