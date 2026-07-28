using System.ComponentModel.DataAnnotations;

namespace GymSystem.BusinessLogic.ViewModels.Members;

public class EditMemberViewModel
{
    public int Id { get; set; }

    //----- Name -----
    [Required(ErrorMessage = "Name is required.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
    public string Name { get; set; } = null!;

    //----- Email -----
    [Required(ErrorMessage = "Email is required.")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;

    //----- Phone Number
    [Required(ErrorMessage = "Phone number is required.")]
    [Display(Name = "Phone Number")]
    [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid phone number format.")]
    public string Phone { get; set; } = null!;

    //----- Date of Birth -----
    [Required(ErrorMessage = "Date of birth is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateOnly DateOfBirth { get; set; }

    //----- Address -----
    [Required(ErrorMessage = "Address is required.")]
    public int BuildingNumber { get; set; }
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
}
