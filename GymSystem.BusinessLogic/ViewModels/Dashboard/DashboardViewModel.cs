namespace GymSystem.BusinessLogic.ViewModels.Dashboard;

public class DashboardViewModel
{

    public int TotalMembers { get; set; }

    public int ActiveMembers { get; set; }

    public int TotalTrainers { get; set; }

    public int UpcomingSessions { get; set; }
    public int OngoingSessions { get; set; }
    public int CompletedSessions { get; set; }

    public int ActivePlans { get; set; }

    public int TotalSessions => UpcomingSessions + OngoingSessions + CompletedSessions;

    public int ActiveMemberPercentage => TotalMembers == 0
        ? 0
        : (int)Math.Round(ActiveMembers * 100d / TotalMembers);
}
