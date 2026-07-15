using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Entities;

public class Membership : BaseEntity
{
    public int PlanId { get; set; }
    public int MemberId { get; set; }
    public DateOnly EndDate { get; set; }
    [NotMapped]
    public bool IsActive => EndDate > DateOnly.FromDateTime(DateTime.Now);




    public Plan Plan { get; set; } = null!;
    public Member Menber { get; set; } = null!;
}
