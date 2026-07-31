using System.ComponentModel.DataAnnotations;

namespace GymSystem.BusinessLogic.ViewModels.Memberships;

public class CreateMembershipViewModel
{

    [Display(Name = "Member")]
    [Required(ErrorMessage = "Member is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a member from the list.")]
    public int? MemberId { get; set; }

    [Display(Name = "Plan")]
    [Required(ErrorMessage = "Plan is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a plan from the list.")]
    public int? PlanId { get; set; }

    public IEnumerable<LookupItemViewModel> Members { get; set; } = [];
    public IEnumerable<LookupItemViewModel> Plans { get; set; } = [];
}
