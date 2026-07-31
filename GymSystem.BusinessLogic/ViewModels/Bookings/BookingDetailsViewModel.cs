using GymSystem.BusinessLogic.ViewModels.Sessions;

namespace GymSystem.BusinessLogic.ViewModels.Bookings;

public class BookingDetailsViewModel
{
    public int Id { get; set; }

    public string MemberName { get; set; } = null!;
    public string MemberEmail { get; set; } = null!;
    public string MemberPhone { get; set; } = null!;

    public string SessionName { get; set; } = null!;
    public string SessionDescription { get; set; } = null!;
    public string TrainerName { get; set; } = null!;
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;

    public int BookedSlots { get; set; }
    public int Capacity { get; set; }
    public string Slots => $"{BookedSlots} / {Capacity}";

    public bool IsAttended { get; set; }
    public SessionStatus SessionStatus { get; set; }

    public bool CanCancel => SessionStatus == SessionStatus.Upcoming;

    public string AttendanceLabel => SessionStatus == SessionStatus.Upcoming
        ? "Not started"
        : IsAttended ? "Attended" : "No show";
}
