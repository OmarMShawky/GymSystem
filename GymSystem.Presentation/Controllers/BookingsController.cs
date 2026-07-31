using GymSystem.BusinessLogic.Common;
using GymSystem.BusinessLogic.Services;
using GymSystem.BusinessLogic.ViewModels.Bookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Presentation.Controllers;

[Authorize]
public class BookingsController(IBookingService bookingService) : Controller
{
    private readonly IBookingService _bookingService = bookingService;

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Message = TempData["Hello from ViewBag"] as string;
        var bookings = await _bookingService.GetBookingsAsync(cancellationToken);
        return View(bookings);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var viewModel = await _bookingService
            .LoadLookupsAsync(new CreateBookingViewModel(), cancellationToken);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookingViewModel createBookingViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(await _bookingService.LoadLookupsAsync(createBookingViewModel, cancellationToken));

        var result = await _bookingService.CreateBookingAsync(createBookingViewModel, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["Hello from ViewBag"] = "Booking created successfully!";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result.Error!);

        return View(await _bookingService.LoadLookupsAsync(createBookingViewModel, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var result = await _bookingService.GetBookingDetailsAsync(id, cancellationToken);

        if (result.IsFailure)
            return this.FromFailure(result);

        return View(result.Value);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleAttendance(int id, CancellationToken cancellationToken)
    {
        var result = await _bookingService.ToggleAttendanceAsync(id, cancellationToken);

        if (result.Status == ResultStatus.NotFound)
            return NotFound();

        TempData["Hello from ViewBag"] = result.IsSuccess
            ? "Attendance updated."
            : result.Error;

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _bookingService.GetBookingDetailsAsync(id, cancellationToken);

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
        var result = await _bookingService.CancelBookingAsync(id, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["Hello from ViewBag"] = "Booking cancelled.";
            return RedirectToAction(nameof(Index));
        }

        if (result.Status == ResultStatus.NotFound)
            return NotFound();

        TempData["Hello from ViewBag"] = result.Error;
        return RedirectToAction(nameof(Delete), new { id });
    }
}
