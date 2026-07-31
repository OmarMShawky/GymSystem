using AutoMapper;

namespace GymSystem.BusinessLogic.Mapping;

public class SessionProfile : Profile
{
    public SessionProfile()
    {

        CreateMap<Session, SessionViewModel>()
            .ForMember(d => d.Specialty, o => o.MapFrom(s =>
                s.Category != null ? s.Category.Name : s.Name))
            .ForMember(d => d.Description, o => o.MapFrom(s =>
                string.IsNullOrWhiteSpace(s.Description)
                    ? (s.Category != null ? s.Category.Description : string.Empty)
                    : s.Description))
            .ForMember(d => d.TrainerName, o => o.MapFrom(s =>
                s.Trainer != null ? s.Trainer.Name : "Unassigned"))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.StartDate.ToString("dd MMM yyyy")))
            .ForMember(d => d.Time, o => o.MapFrom(s =>
                s.StartDate.ToString("hh:mm tt") + " - " + s.EndDate.ToString("hh:mm tt")))
            .ForMember(d => d.Duration, o => o.MapFrom(s => FormatDuration(s.EndDate - s.StartDate)))
            .ForMember(d => d.BookedSlots, o => o.MapFrom(s =>
                s.SessionMembers != null ? s.SessionMembers.Count : 0))
            .ForMember(d => d.Status, o => o.MapFrom(s => ResolveStatus(s.StartDate, s.EndDate)));

        CreateMap<Session, SessionDetailsViewModel>()
            .ForMember(d => d.Category, o => o.MapFrom(s =>
                s.Category != null ? s.Category.Name : s.Name))
            .ForMember(d => d.TrainerName, o => o.MapFrom(s =>
                s.Trainer != null ? s.Trainer.Name : "Unassigned"))
            .ForMember(d => d.StartTime, o => o.MapFrom(s => s.StartDate.ToString("dd MMM yyyy, hh:mm tt")))
            .ForMember(d => d.EndTime, o => o.MapFrom(s => s.EndDate.ToString("dd MMM yyyy, hh:mm tt")))
            .ForMember(d => d.Duration, o => o.MapFrom(s => FormatLongDuration(s.EndDate - s.StartDate)))
            .ForMember(d => d.BookedSlots, o => o.MapFrom(s =>
                s.SessionMembers != null ? s.SessionMembers.Count : 0))
            .ForMember(d => d.Status, o => o.MapFrom(s => ResolveStatus(s.StartDate, s.EndDate)));

        CreateMap<Session, EditSessionViewModel>()
            .ForMember(d => d.Category, o => o.MapFrom(s =>
                s.Category != null ? s.Category.Name : s.Name))
            .ForMember(d => d.Status, o => o.MapFrom(s => ResolveStatus(s.StartDate, s.EndDate)))
            .ForMember(d => d.Trainers, o => o.Ignore());

        CreateMap<CreateSessionViewModel, Session>()
            .ForMember(d => d.Name, o => o.Ignore())
            .ForMember(d => d.Category, o => o.Ignore())
            .ForMember(d => d.Trainer, o => o.Ignore())
            .ForMember(d => d.SessionMembers, o => o.Ignore());

        CreateMap<Category, LookupItemViewModel>();
    }

    private static SessionStatus ResolveStatus(DateTime start, DateTime end)
    {
        var now = DateTime.Now;

        if (now < start)
            return SessionStatus.Upcoming;

        return now <= end ? SessionStatus.Ongoing : SessionStatus.Completed;
    }

    private static string FormatLongDuration(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
            return "-";

        var hours = (int)duration.TotalHours;
        var minutes = duration.Minutes;

        var parts = new List<string>();

        if (hours > 0)
            parts.Add($"{hours} {(hours == 1 ? "Hour" : "Hours")}");

        if (minutes > 0)
            parts.Add($"{minutes} {(minutes == 1 ? "Minute" : "Minutes")}");

        return string.Join(" ", parts);
    }

    private static string FormatDuration(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
            return "-";

        var hours = (int)duration.TotalHours;
        var minutes = duration.Minutes;

        if (hours == 0)
            return $"{minutes}m";

        return minutes == 0 ? $"{hours}h" : $"{hours}h {minutes}m";
    }
}
