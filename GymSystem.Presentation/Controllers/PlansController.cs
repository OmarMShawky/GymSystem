using GymSystem.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Presentation.Controllers;

public class PlansController : Controller
{
    private readonly IPlansService _service;

    public PlansController(IPlansService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var plans = await _service.PlansAsync(cancellationToken);
        return View(plans);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return RedirectToAction(nameof(Index));
        }

        var plan = await _service.GetPlanByIdAsync(id, cancellationToken);

        if (plan == null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(plan);
    }
}


//using GymSystem.BusinessLogic.Services;
//using GymSystem.DataAccess.Data;
//using GymSystem.DataAccess.Repositories;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace GymSystem.Presentation.Controllers;

//public class PlansController : Controller
//{
//    private readonly PlansService _service;

//    public PlansController(PlansService service)
//    {
//        _service = service;

//    [HttpGet]
//    public async Task<IActionResult> Index(CancellationToken cancellationToken)
//    {
//        var plans = await _service.PlansAsync(cancellationToken);
//        return View(plans);
//    }

//    [HttpGet]
//    public async Task<IActionResult> Details(int id)
//    {
//        if (id <= 0)
//        {
//            return RedirectToAction(nameof(Index));
//        }

//        var plan = _service..GetByIdAsync(id);

//        if (plan == null)
//        {
//            return RedirectToAction(nameof(Index));
//        }

//        return View(plan);
//    }
//}
