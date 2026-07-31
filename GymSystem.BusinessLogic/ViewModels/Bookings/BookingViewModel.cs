using GymSystem.BusinessLogic.ViewModels.Sessions;

namespace GymSystem.BusinessLogic.ViewModels.Bookings;

public class BookingViewModel
{
    public int Id { get; set; }

    public int MemberId { get; set; }
    public string MemberName { get; set; } = null!;

    public int SessionId { get; set; }

    public string SessionName { get; set; } = null!;

    public string TrainerName { get; set; } = null!;

    public string Date { get; set; } = null!;
    public string Time { get; set; } = null!;

    public bool IsAttended { get; set; }
    public SessionStatus SessionStatus { get; set; }

    public bool CanMarkAttendance => SessionStatus != SessionStatus.Upcoming;

    public bool CanCancel => SessionStatus == SessionStatus.Upcoming;

    public string AttendanceLabel => SessionStatus == SessionStatus.Upcoming
        ? "Not started"
        : IsAttended ? "Attended" : "No show";
}
