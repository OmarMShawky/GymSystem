using AutoMapper;
using GymSystem.BusinessLogic.ViewModels.Bookings;
using GymSystem.BusinessLogic.ViewModels.Sessions;

namespace GymSystem.BusinessLogic.Mapping;

public class BookingProfile : Profile
{
    public BookingProfile()
    {

        CreateMap<Booking, BookingViewModel>()
            .ForMember(d => d.MemberName, o => o.MapFrom(s => s.Member != null ? s.Member.Name : "Unknown"))
            .ForMember(d => d.SessionName, o => o.MapFrom(s =>
                s.Session == null ? "Unknown"
                : s.Session.Category != null ? s.Session.Category.Name : s.Session.Name))
            .ForMember(d => d.TrainerName, o => o.MapFrom(s =>
                s.Session != null && s.Session.Trainer != null ? s.Session.Trainer.Name : "Unassigned"))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.Session.StartDate.ToString("dd MMM yyyy")))
            .ForMember(d => d.Time, o => o.MapFrom(s =>
                s.Session.StartDate.ToString("hh:mm tt") + " - " + s.Session.EndDate.ToString("hh:mm tt")))
            .ForMember(d => d.SessionStatus, o => o.MapFrom(s =>
                ResolveStatus(s.Session.StartDate, s.Session.EndDate)));

        CreateMap<Booking, BookingDetailsViewModel>()
            .ForMember(d => d.MemberName, o => o.MapFrom(s => s.Member != null ? s.Member.Name : "Unknown"))
            .ForMember(d => d.MemberEmail, o => o.MapFrom(s => s.Member != null ? s.Member.Email : string.Empty))
            .ForMember(d => d.MemberPhone, o => o.MapFrom(s => s.Member != null ? s.Member.Phone : string.Empty))
            .ForMember(d => d.SessionName, o => o.MapFrom(s =>
                s.Session == null ? "Unknown"
                : s.Session.Category != null ? s.Session.Category.Name : s.Session.Name))
            .ForMember(d => d.SessionDescription, o => o.MapFrom(s => s.Session != null ? s.Session.Description : string.Empty))
            .ForMember(d => d.TrainerName, o => o.MapFrom(s =>
                s.Session != null && s.Session.Trainer != null ? s.Session.Trainer.Name : "Unassigned"))
            .ForMember(d => d.StartTime, o => o.MapFrom(s => s.Session.StartDate.ToString("dd MMM yyyy, hh:mm tt")))
            .ForMember(d => d.EndTime, o => o.MapFrom(s => s.Session.EndDate.ToString("dd MMM yyyy, hh:mm tt")))
            .ForMember(d => d.Capacity, o => o.MapFrom(s => s.Session != null ? s.Session.Capacity : 0))
            .ForMember(d => d.BookedSlots, o => o.Ignore())
            .ForMember(d => d.SessionStatus, o => o.MapFrom(s =>
                ResolveStatus(s.Session.StartDate, s.Session.EndDate)));
    }

    private static SessionStatus ResolveStatus(DateTime start, DateTime end)
    {
        var now = DateTime.Now;

        if (now < start)
            return SessionStatus.Upcoming;

        return now <= end ? SessionStatus.Ongoing : SessionStatus.Completed;
    }
}
