using GymSystem.DataAccess.Contracts;
using GymSystem.DataAccess.Data;
using System.Linq.Expressions;

namespace GymSystem.DataAccess.Repositories;

public class MemberRepository : GenericRepository<Member>, IMemberRepository
{

    public MemberRepository(GymDbContext context) : base(context)
    {
    }

    public Task<bool> EmailExists(string email, CancellationToken cancellationToken = default)
        => _context.Members.AnyAsync(m => m.Email == email, cancellationToken);

    public Task<bool> PhoneExists(string phone, CancellationToken cancellationToken = default)
        => _context.Members.AnyAsync(m => m.Phone == phone, cancellationToken);

}
