using GymSystem.BusinessLogic.Services;
using GymSystem.BusinessLogic.ViewModels.Members;
using Microsoft.AspNetCore.Mvc;
namespace GymSystem.Presentation.Controllers;

public class MembersController : Controller
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public async Task<IActionResult> Index()
    {
        var members = await _memberService.GetMembersAsync();
        return View(members);
    }

    // create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateMemberViewModel memberViewModel)
    {
        if(!ModelState.IsValid)
            return View(memberViewModel);

        // call service to create member
        var result = await _memberService.CreateMemberAsync(memberViewModel);
        if(result)
            return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, "Email or Phone already exists.");

        return View(memberViewModel);
    }
}
