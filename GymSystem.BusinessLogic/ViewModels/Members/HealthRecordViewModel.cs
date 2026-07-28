using GymSystem.DataAccess.Enums;

namespace GymSystem.BusinessLogic.ViewModels.Members;

public class HealthRecordViewModel
{
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public int Age { get; set; }
    public BloodType BloodType { get; set; }
    public string? Notes { get; set; }
}
