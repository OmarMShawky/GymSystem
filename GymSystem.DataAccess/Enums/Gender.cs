using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Enums;

public enum Gender
{
    [Display(Name = "Male")]
    Male = 1,

    [Display(Name = "Female")]
    Female = 2
}
