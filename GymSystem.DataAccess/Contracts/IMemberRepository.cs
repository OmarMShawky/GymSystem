using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Contracts;

public interface IMemberRepository : IGenericRepository<Member>
{
    Task<IEnumerable<Member>> GetAllAsync(string name, bool trackChanges = false, CancellationToken cancellationToken = default);
}
