using GymSystem.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GymSystem.DataAccess.Data;

public class GymDbContext : IdentityDbContext<ApplicationUser>
{
    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
    {
    }

    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<HealthRecord> HealthRecords => Set<HealthRecord>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Trainer> Trainers => Set<Trainer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);
    }
}
