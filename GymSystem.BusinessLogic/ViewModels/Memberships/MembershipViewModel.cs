namespace GymSystem.BusinessLogic.ViewModels.Memberships;

public class MembershipViewModel
{
    public int Id { get; set; }
    public string MemberName { get; set; } = null!;
    public string PlanName { get; set; } = null!;
    public decimal Price { get; set; }

    public string StartDate { get; set; } = null!;
    public string EndDate { get; set; } = null!;

    public bool IsActive { get; set; }
    public string Status => IsActive ? "Active" : "Expired";

    public int DaysRemaining { get; set; }
}
