using GymSystem.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.BusinessLogic.ViewModels.Trainers;

public class EditTrainerViewModel
{
    public int Id { get; set; }

    //----- Locked fields: displayed as read-only context, never updated -----
    public string Name { get; set; } = null!;
    public string DateOfBirth { get; set; } = null!;
    public string Gender { get; set; } = null!;

    //----- Editable: contact -----
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Phone Number")]
    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^(010|011|012|015)\d{8}$",
        ErrorMessage = "Phone must be a valid Egyptian number, e.g. 01012345678.")]
    public string Phone { get; set; } = null!;

    //----- Editable: address -----
    [Display(Name = "Building Number")]
    [Required(ErrorMessage = "Building number is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Building number must be a positive value.")]
    public int BuildingNumber { get; set; }

    [Required(ErrorMessage = "Street is required.")]
    public string Street { get; set; } = null!;

    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; } = null!;

    //----- Editable: professional -----
    [Display(Name = "Specialty")]
    [Required(ErrorMessage = "Specialty is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a specialty from the list.")]
    public int? CategoryId { get; set; }

    /// <summary>Dropdown data, repopulated by the controller on every render.</summary>
    public IEnumerable<LookupItemViewModel> Categories { get; set; } = [];
}
