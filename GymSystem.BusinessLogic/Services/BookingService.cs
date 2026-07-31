using AutoMapper;
using GymSystem.BusinessLogic.ViewModels.Bookings;
using System.Linq.Expressions;

namespace GymSystem.BusinessLogic.Services;

public class BookingService(IUnitOfWork unitOfWork, IMapper mapper) : IBookingService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    private static readonly Expression<Func<Booking, object>>[] _includes =
    [
        b => b.Member,
        b => b.Session,
        b => b.Session.Category,
        b => b.Session.Trainer
    ];

    public async Task<IEnumerable<BookingViewModel>> GetBookingsAsync(CancellationToken cancellationToken = default)
    {
        var bookings = await _unitOfWork.GetRepository<Booking>()
            .GetAllWithIncludesAsync(_includes, cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<BookingViewModel>>(
            bookings.OrderBy(b => b.Session.StartDate));
    }

    public async Task<CreateBookingViewModel> LoadLookupsAsync(
        CreateBookingViewModel createBookingViewModel, CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;

        var members = await _unitOfWork.GetRepository<Member>()
            .GetAllAsync(cancellationToken: cancellationToken);

        var sessions = await _unitOfWork.GetRepository<Session>()
            .GetAllWithIncludesAsync(
                [s => s.Category, s => s.Trainer, s => s.SessionMembers],
                cancellationToken: cancellationToken);

        createBookingViewModel.Members = members
            .OrderBy(m => m.Name)
            .Select(m => new LookupItemViewModel { Id = m.Id, Name = $"{m.Name} ({m.Email})" })
            .ToList();

        createBookingViewModel.Sessions = sessions
            .Where(s => s.StartDate > now && (s.SessionMembers?.Count ?? 0) < s.Capacity)
            .OrderBy(s => s.StartDate)
            .Select(s => new LookupItemViewModel
            {
                Id = s.Id,
                Name = $"{(s.Category != null ? s.Category.Name : s.Name)} - " +
                       $"{s.StartDate:dd MMM, hh:mm tt} - " +
                       $"{(s.Trainer != null ? s.Trainer.Name : "Unassigned")} " +
                       $"({s.SessionMembers?.Count ?? 0}/{s.Capacity})"
            })
            .ToList();

        return createBookingViewModel;
    }

    public async Task<Result> CreateBookingAsync(
        CreateBookingViewModel createBookingViewModel, CancellationToken cancellationToken = default)
    {

        var memberId = createBookingViewModel.MemberId!.Value;
        var sessionId = createBookingViewModel.SessionId!.Value;

        var member = await _unitOfWork.GetRepository<Member>()
            .GetByIdAsync(memberId, cancellationToken);

        if (member is null)
            return Result.NotFound("The selected member no longer exists.");

        var session = await _unitOfWork.GetRepository<Session>()
            .GetByIdAsync(sessionId, cancellationToken);

        if (session is null)
            return Result.NotFound("The selected session no longer exists.");

        if (session.StartDate <= DateTime.Now)
            return Result.Fail("That session has already started and can no longer be booked.");

        var bookingRepo = _unitOfWork.GetRepository<Booking>();

        var alreadyBooked = await bookingRepo
            .AnyAsync(b => b.MemberId == memberId && b.SessionId == sessionId, cancellationToken);

        if (alreadyBooked)
            return Result.Conflict("This member is already booked on that session.");

        var booked = await bookingRepo.CountAsync(b => b.SessionId == sessionId, cancellationToken);

        if (booked >= session.Capacity)
            return Result.Conflict("That session is fully booked.");

        var today = DateOnly.FromDateTime(DateTime.Now);

        var hasActiveMembership = await _unitOfWork.GetRepository<Membership>()
            .AnyAsync(m => m.MemberId == memberId && m.EndDate > today, cancellationToken);

        if (!hasActiveMembership)
            return Result.Fail("This member has no active membership and cannot book a session.");

        bookingRepo.Add(new Booking
        {
            MemberId = memberId,
            SessionId = sessionId,
            IsAttended = false
        }, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0
            ? Result.Ok()
            : Result.Fail("The booking could not be created.");
    }

    public async Task<Result<BookingDetailsViewModel>> GetBookingDetailsAsync(
        int id, CancellationToken cancellationToken = default)
    {
        var booking = await _unitOfWork.GetRepository<Booking>()
            .GetByIdWithIncludesAsync(id, _includes, cancellationToken);

        if (booking is null)
            return Result.NotFound<BookingDetailsViewModel>("Booking not found.");

        var details = _mapper.Map<BookingDetailsViewModel>(booking);

        details.BookedSlots = await _unitOfWork.GetRepository<Booking>()
            .CountAsync(b => b.SessionId == booking.SessionId, cancellationToken);

        return details;
    }

    public async Task<Result> ToggleAttendanceAsync(int id, CancellationToken cancellationToken = default)
    {
        var bookingRepo = _unitOfWork.GetRepository<Booking>();

        var booking = await bookingRepo.GetByIdWithIncludesAsync(id, [b => b.Session], cancellationToken);

        if (booking is null)
            return Result.NotFound("Booking not found.");

        if (booking.Session.StartDate > DateTime.Now)
            return Result.Fail("Attendance can only be recorded once the session has started.");

        var tracked = await bookingRepo.GetByIdAsync(id, cancellationToken);

        if (tracked is null)
            return Result.NotFound("Booking not found.");

        tracked.IsAttended = !tracked.IsAttended;

        bookingRepo.Update(tracked, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0
            ? Result.Ok()
            : Result.Fail("Attendance could not be updated.");
    }

    public async Task<Result> CancelBookingAsync(int id, CancellationToken cancellationToken = default)
    {
        var bookingRepo = _unitOfWork.GetRepository<Booking>();

        var booking = await bookingRepo.GetByIdWithIncludesAsync(id, [b => b.Session], cancellationToken);

        if (booking is null)
            return Result.NotFound("Booking not found.");

        if (booking.Session.StartDate <= DateTime.Now)
            return Result.Conflict("A booking can only be cancelled before the session starts.");

        var tracked = await bookingRepo.GetByIdAsync(id, cancellationToken);

        if (tracked is null)
            return Result.NotFound("Booking not found.");

        bookingRepo.Delete(tracked, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0
            ? Result.Ok()
            : Result.Fail("The booking could not be cancelled.");
    }
}
