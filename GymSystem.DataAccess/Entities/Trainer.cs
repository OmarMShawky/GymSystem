using GymSystem.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Entities;

public class Trainer : GymUser
{
    public Specialty Specialty { get; set; }
    public DateTime HireDate { get; set; }

    public ICollection<Session> Sessions{ get; set; } = [];
}
