using GymSystem.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.BusinessLogic.ViewModels.Trainers;

public class CreateTrainerViewModel
{

    [Required(ErrorMessage = "Name is required.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Phone Number")]
    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^(010|011|012|015)\d{8}$",
        ErrorMessage = "Phone must be a valid Egyptian number, e.g. 01012345678.")]
    public string Phone { get; set; } = null!;

    [Display(Name = "Date of Birth")]
    [Required(ErrorMessage = "Date of birth is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    [EnumDataType(typeof(Gender), ErrorMessage = "Select a gender from the list.")]
    public Gender Gender { get; set; }

    [Display(Name = "Building Number")]
    [Required(ErrorMessage = "Building number is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Building number must be a positive value.")]
    public int BuildingNumber { get; set; }

    [Required(ErrorMessage = "Street is required.")]
    public string Street { get; set; } = null!;

    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; } = null!;

    [Display(Name = "Specialty")]
    [Required(ErrorMessage = "Specialty is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a specialty from the list.")]
    public int? CategoryId { get; set; }

    public IEnumerable<LookupItemViewModel> Categories { get; set; } = [];
}
