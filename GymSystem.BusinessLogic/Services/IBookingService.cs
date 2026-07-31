using GymSystem.BusinessLogic.ViewModels.Bookings;

namespace GymSystem.BusinessLogic.Services;

public interface IBookingService
{

    Task<IEnumerable<BookingViewModel>> GetBookingsAsync(CancellationToken cancellationToken = default);

    Task<CreateBookingViewModel> LoadLookupsAsync(
        CreateBookingViewModel createBookingViewModel, CancellationToken cancellationToken = default);

    Task<Result> CreateBookingAsync(
        CreateBookingViewModel createBookingViewModel, CancellationToken cancellationToken = default);

    Task<Result<BookingDetailsViewModel>> GetBookingDetailsAsync(int id, CancellationToken cancellationToken = default);

    Task<Result> ToggleAttendanceAsync(int id, CancellationToken cancellationToken = default);

    Task<Result> CancelBookingAsync(int id, CancellationToken cancellationToken = default);
}
