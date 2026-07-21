using GymSystem.DataAccess.Data;
using GymSystem.DataAccess.Contracts;
using GymSystem.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.DataAccess.Repositories;

public class PlansRepository : GenericRepository<Plan>, IGenericRepository<Plan>
{
    public PlansRepository(GymDbContext context) : base(context)
    {

    }

}
