namespace GymSystem.BusinessLogic.ViewModels.Sessions;

public class SessionViewModel
{
    public int Id { get; set; }
    public string Specialty { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string TrainerName { get; set; } = null!;
    public string Date { get; set; } = null!;
    public string Time { get; set; } = null!;
    public string Duration { get; set; } = null!;
    public int BookedSlots { get; set; }
    public int Capacity { get; set; }
    public string Slots => $"{BookedSlots}/{Capacity}";
    public bool IsFull => BookedSlots >= Capacity;
    public SessionStatus Status { get; set; }
}
