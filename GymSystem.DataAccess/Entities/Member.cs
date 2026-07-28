namespace GymSystem.DataAccess.Entities;

public class Member : GymUser
{
    public string? Photo { get; set; }
    public DateTime JoinDate { get; set; }
    public ICollection<Membership> MemberPlans { get; set; } = [];
    public ICollection<Booking> MemberSessions { get; set; } = [];
    public HealthRecord HealthRecord { get; set; } = null!;

}
