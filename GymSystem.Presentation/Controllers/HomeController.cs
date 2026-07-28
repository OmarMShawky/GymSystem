using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Presentation.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
