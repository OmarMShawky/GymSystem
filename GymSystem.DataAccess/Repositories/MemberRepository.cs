using GymSystem.DataAccess.Contracts;
using GymSystem.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Repositories;

public class MemberRepository : GenericRepository<Member>, IMemberRepository
{

    public MemberRepository(GymDbContext context) : base(context)
    {
    }
    public Task<IEnumerable<Member>> GetAllAsync(string name, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
