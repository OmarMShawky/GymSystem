using GymSystem.BusinessLogic.ViewModels.Account;
using GymSystem.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Presentation.Controllers;

[AllowAnonymous]
public class AccountController(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager) : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    private const string GenericLoginError = "Invalid email or password.";

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (_signInManager.IsSignedIn(User))
            return RedirectToLocal(returnUrl);

        ViewData["ReturnUrl"] = returnUrl;

        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(loginViewModel);

        var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

        if (user is null)
        {

            ModelState.AddModelError("InvalidLogin", GenericLoginError);
            return View(loginViewModel);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user,
            loginViewModel.Password,
            isPersistent: loginViewModel.RememberMe,
            lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(
                "InvalidLogin",
                "This account is temporarily locked after too many failed attempts. Please try again in a couple of minutes.");

            return View(loginViewModel);
        }

        if (!result.Succeeded)
        {
            ModelState.AddModelError("InvalidLogin", GenericLoginError);
            return View(loginViewModel);
        }

        return RedirectToLocal(returnUrl);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();

    private IActionResult RedirectToLocal(string? returnUrl)
        => Url.IsLocalUrl(returnUrl)
            ? Redirect(returnUrl!)
            : RedirectToAction("Index", "Home");
}
