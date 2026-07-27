using System.ComponentModel.DataAnnotations.Schema;

namespace GymSystem.DataAccess.Entities;

public class Membership : BaseEntity
{
    public int PlanId { get; set; }
    public int MemberId { get; set; }
    public DateOnly EndDate { get; set; }
    public Plan Plan { get; set; } = null!;
    public Member Member { get; set; } = null!;
    [NotMapped]
    public bool IsActive => EndDate > DateOnly.FromDateTime(DateTime.Now);
}
