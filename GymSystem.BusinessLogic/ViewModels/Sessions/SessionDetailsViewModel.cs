namespace GymSystem.BusinessLogic.ViewModels.Sessions;

public class SessionDetailsViewModel
{
    public int Id { get; set; }

    /// <summary>Category name shown in the card header.</summary>
    public string Category { get; set; } = null!;

    public string Description { get; set; } = null!;
    public string TrainerName { get; set; } = null!;

    /// <summary>Full timestamp, e.g. "30 Jul 2026, 05:00 PM".</summary>
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;

    /// <summary>Computed from EndTime - StartTime, e.g. "1 Hour 30 Minutes".</summary>
    public string Duration { get; set; } = null!;

    public int BookedSlots { get; set; }
    public int Capacity { get; set; }

    /// <summary>Booked / max spots, e.g. "0 / 2".</summary>
    public string Slots => $"{BookedSlots} / {Capacity}";

    public SessionStatus Status { get; set; }
}
