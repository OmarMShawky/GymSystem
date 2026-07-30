namespace GymSystem.BusinessLogic.ViewModels;

/// <summary>
/// A single dropdown option. Kept framework-free so the business layer
/// does not need a reference to ASP.NET's SelectListItem.
/// </summary>
public class LookupItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
