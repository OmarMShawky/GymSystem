using GymSystem.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymSystem.DataAccess.Entities;

public class GymUser : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;

    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public Address Address { get; set; } = null!;

    [NotMapped]
    public int Age
    {
        get
        {
            if (DateOfBirth == default)
                return 0;

            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - DateOfBirth.Year;

            if (DateOfBirth > today.AddYears(-age))
                age--;

            return age;
        }
    }
}

[Owned]
public class Address
{
    public int BuildingNumber { get; set; }
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
}
