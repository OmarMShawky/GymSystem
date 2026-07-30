using System.ComponentModel.DataAnnotations;

namespace GymSystem.BusinessLogic.ViewModels.Sessions;

public class CreateSessionViewModel : IValidatableObject
{
    //----- Session Information -----
    // Nullable so an empty post binds to null and surfaces the [Required] message,
    // instead of the framework's generic "The value '' is invalid."
    [Display(Name = "Category")]
    [Required(ErrorMessage = "Category is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a category from the list.")]
    public int? CategoryId { get; set; }

    [Display(Name = "Trainer")]
    [Required(ErrorMessage = "Trainer is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a trainer from the list.")]
    public int? TrainerId { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
    public string Description { get; set; } = null!;

    [Display(Name = "Capacity")]
    [Required(ErrorMessage = "Capacity is required.")]
    [Range(1, 25, ErrorMessage = "Capacity must be between 1 and 25 participants.")]
    public int? Capacity { get; set; }

    //----- Date & Time -----
    [Display(Name = "Start Date & Time")]
    [Required(ErrorMessage = "Start date and time is required.")]
    [DataType(DataType.DateTime)]
    public DateTime? StartDate { get; set; }

    [Display(Name = "End Date & Time")]
    [Required(ErrorMessage = "End date and time is required.")]
    [DataType(DataType.DateTime)]
    public DateTime? EndDate { get; set; }

    //----- Dropdown data, repopulated by the controller on every render -----
    public IEnumerable<LookupItemViewModel> Categories { get; set; } = [];
    public IEnumerable<LookupItemViewModel> Trainers { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate is { } start && start <= DateTime.Now)
        {
            yield return new ValidationResult(
                "Start date and time must be in the future.",
                [nameof(StartDate)]);
        }

        if (StartDate is { } from && EndDate is { } to && to <= from)
        {
            yield return new ValidationResult(
                "End date and time must be after the start date and time.",
                [nameof(EndDate)]);
        }
    }
}
