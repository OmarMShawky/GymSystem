using System.ComponentModel.DataAnnotations;

namespace GymSystem.DataAccess.Enums;

public enum BloodType
{
    [Display(Name = "Unknown")]
    Unknown = 0,

    [Display(Name = "A+")]
    APositive = 1,

    [Display(Name = "A-")]
    ANegative = 2,

    [Display(Name = "B+")]
    BPositive = 3,

    [Display(Name = "B-")]
    BNegative = 4,

    [Display(Name = "AB+")]
    ABPositive = 5,

    [Display(Name = "AB-")]
    ABNegative = 6,

    [Display(Name = "O+")]
    OPositive = 7,

    [Display(Name = "O-")]
    ONegative = 8
}
