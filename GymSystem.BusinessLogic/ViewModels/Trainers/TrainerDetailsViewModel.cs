namespace GymSystem.BusinessLogic.ViewModels.Trainers;

public class TrainerDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    /// <summary>Rendered as "{Specialty} Trainer", e.g. "Boxing Trainer".</summary>
    public string Specialization { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string DateOfBirth { get; set; } = null!;
    public string Gender { get; set; } = null!;

    //----- Address parts, joined in the view -----
    public int BuildingNumber { get; set; }
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
}
