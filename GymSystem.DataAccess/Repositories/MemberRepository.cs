using GymSystem.DataAccess.Contracts;
using GymSystem.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Repositories;

public class MemberRepository : GenericRepository<Member>, IMemberRepository
{

    public MemberRepository(GymDbContext context) : base(context)
    {
    }

    public Task<bool> AnyAsync(Expression<Func<Member, bool>> predicate, CancellationToken cancellationToken = default)
        => _context.Members.AnyAsync(predicate, cancellationToken);


    public Task<bool> EmailExists(string email, CancellationToken cancellationToken = default)
        => _context.Members.AnyAsync(m => m.Email == email, cancellationToken);


    public Task<bool> PhoneExists(string phone, CancellationToken cancellationToken = default)
        => _context.Members.AnyAsync(m => m.Phone == phone, cancellationToken);

}
