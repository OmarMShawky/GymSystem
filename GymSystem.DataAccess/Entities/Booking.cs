namespace GymSystem.DataAccess.Entities;

public class Booking : BaseEntity
{
    public int MemberId { get; set; }
    public int SessionId { get; set; }
    public bool IsAttended { get; set; }
    public Member Member { get; set; } = null!;
    public Session Session { get; set; } = null!;
}
