using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Entities;

public class Session : BaseEntity
{
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CategoryId { get; set; }
    public int TrainerId { get; set; }
    public Category Category { get; set; } = null!;
    public Trainer Trainer { get; set; } = null!;
    public ICollection<Booking> SessionMembers { get; set; } = [];

}
