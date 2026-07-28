namespace GymSystem.BusinessLogic.ViewModels.Members;

public class MemberViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Photo { get; set; } = string.Empty;
}
