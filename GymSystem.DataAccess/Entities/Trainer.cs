using GymSystem.DataAccess.Enums;

namespace GymSystem.DataAccess.Entities;

public class Trainer : GymUser
{
    public Specialty Specialty { get; set; }
    public DateTime HireDate { get; set; }

    public ICollection<Session> Sessions { get; set; } = [];
}
