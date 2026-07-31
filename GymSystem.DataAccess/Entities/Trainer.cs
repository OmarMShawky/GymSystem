namespace GymSystem.DataAccess.Entities;

public class Trainer : GymUser
{

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public DateTime HireDate { get; set; }

    public ICollection<Session> Sessions { get; set; } = [];
}
