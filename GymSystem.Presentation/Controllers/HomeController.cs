using GymSystem.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymSystem.Presentation.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
