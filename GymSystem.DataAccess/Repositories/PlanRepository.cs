using GymSystem.DataAccess.Contracts;
using GymSystem.DataAccess.Data;

namespace GymSystem.DataAccess.Repositories;

public class PlansRepository : GenericRepository<Plan>, IGenericRepository<Plan>
{
    public PlansRepository(GymDbContext context) : base(context)
    {

    }

}
