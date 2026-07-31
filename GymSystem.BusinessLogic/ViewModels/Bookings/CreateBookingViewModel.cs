using System.ComponentModel.DataAnnotations;

namespace GymSystem.BusinessLogic.ViewModels.Bookings;

public class CreateBookingViewModel
{

    [Display(Name = "Member")]
    [Required(ErrorMessage = "Member is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a member from the list.")]
    public int? MemberId { get; set; }

    [Display(Name = "Session")]
    [Required(ErrorMessage = "Session is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a session from the list.")]
    public int? SessionId { get; set; }

    public IEnumerable<LookupItemViewModel> Members { get; set; } = [];
    public IEnumerable<LookupItemViewModel> Sessions { get; set; } = [];
}
