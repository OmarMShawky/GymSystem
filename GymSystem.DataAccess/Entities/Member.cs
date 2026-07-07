using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Entities;

public class Member : User
{
    public string? Photo { get; set; }
    public DateTime JoinDate { get; set; }

    public HealthRecord HealthRecord { get; set; } = null!;

    // ICollection<Booking>

    // ICollection<MemeberShip>
}
