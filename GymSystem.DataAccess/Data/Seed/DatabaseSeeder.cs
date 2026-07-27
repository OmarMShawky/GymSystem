namespace GymSystem.DataAccess.Data.Seed;

public class DatabaseSeeder
{
    public static async Task SeedAllAsync(GymDbContext dbContext)
    {
        await PlanSeeder.SeedAsync(dbContext);
        await CategorySeeder.SeedAsync(dbContext);
    }
}
