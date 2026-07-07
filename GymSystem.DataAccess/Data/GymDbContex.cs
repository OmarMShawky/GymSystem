using GymSystem.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.DataAccess.Data;

public class GymDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);
    }

    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<User> Users => Set<User>();
}
