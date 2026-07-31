using GymSystem.DataAccess.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.BusinessLogic.ViewModels.Members;

public class CreateMemberViewModel
{

    [Required(ErrorMessage = "Name is required.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Phone number is required.")]
    [Display(Name = "Phone Number")]
    [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid phone number format.")]
    public string Phone { get; set; } = null!;

    [Required(ErrorMessage = "Date of birth is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    public Gender Gender { get; set; }

    [Display(Name = "Photo")]
    [Required(ErrorMessage = "A profile photo is required.")]
    public IFormFile Photo { get; set; } = null!;

    [Required(ErrorMessage = "Address is required.")]
    public int BuildingNumber { get; set; }
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;

    [Required(ErrorMessage = "Health record is required.")]
    public HealthRecordViewModel HealthRecord { get; set; } = new HealthRecordViewModel();

}
