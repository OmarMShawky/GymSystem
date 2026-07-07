using GymSystem.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Entities;

public class Trainer : User
{
    public Specialty Specialty { get; set; }
    public DateTime HireDate { get; set; }

    // ICollection<Session>
}
