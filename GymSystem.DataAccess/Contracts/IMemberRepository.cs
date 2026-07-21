using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Contracts;

public interface IMemberRepository : IGenericRepository<Member>
{
    Task<bool> AnyAsync(Expression<Func<Member, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> EmailExists(string email, CancellationToken cancellationToken = default);
    Task<bool> PhoneExists(string phone, CancellationToken cancellationToken = default);
}
