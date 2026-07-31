using System.ComponentModel.DataAnnotations;

namespace GymSystem.BusinessLogic.ViewModels.Plans;

public class EditPlanViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; } = null!;

    [Display(Name = "Duration (days)")]
    [Range(1, 3650, ErrorMessage = "Duration must be between 1 and 3650 days.")]
    public int DurationDays { get; set; }

    [Display(Name = "Price (EGP)")]
    [Range(0, 1_000_000, ErrorMessage = "Price must be a positive value.")]
    public decimal Price { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; }
}
