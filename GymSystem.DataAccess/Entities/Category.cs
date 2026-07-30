namespace GymSystem.DataAccess.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ICollection<Session> Sessions { get; set; } = [];
    public ICollection<Trainer> Trainers { get; set; } = [];
}
