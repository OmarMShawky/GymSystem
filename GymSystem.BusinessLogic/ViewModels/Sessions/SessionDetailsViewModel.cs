namespace GymSystem.BusinessLogic.ViewModels.Sessions;

public class SessionDetailsViewModel
{
    public int Id { get; set; }

    public string Category { get; set; } = null!;

    public string Description { get; set; } = null!;
    public string TrainerName { get; set; } = null!;

    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;

    public string Duration { get; set; } = null!;

    public int BookedSlots { get; set; }
    public int Capacity { get; set; }

    public string Slots => $"{BookedSlots} / {Capacity}";

    public SessionStatus Status { get; set; }
}
