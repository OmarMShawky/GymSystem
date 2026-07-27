using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GymSystem.BusinessLogic.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Returns the value's [Display(Name = "...")] text, falling back to the raw enum name.
    /// </summary>
    public static string ToDisplayName(this Enum value)
    {
        var member = value.GetType()
                          .GetMember(value.ToString())
                          .FirstOrDefault();

        var display = member?.GetCustomAttribute<DisplayAttribute>();

        return display?.Name ?? value.ToString();
    }
}
