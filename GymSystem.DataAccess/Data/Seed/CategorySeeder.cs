namespace GymSystem.DataAccess.Data.Seed;

public static class CategorySeeder
{
    public static async Task SeedAsync(GymDbContext dbContext)
    {
        await dbContext.Database.MigrateAsync();

        if (await dbContext.Categories.AnyAsync())
        {
            return;
        }

        var categories = new List<Category>
        {
            new() { Name = "CrossFit",  Description = "High-intensity functional training combining strength and cardio." },
            new() { Name = "Strength",  Description = "Weight-based training focused on building muscle and power." },
            new() { Name = "Boxing",    Description = "Combat-inspired workouts focused on technique, speed, and conditioning." },
            new() { Name = "Dancing",   Description = "Cardio and rhythm-based classes for fitness and fun." },
            new() { Name = "Yoga",      Description = "Mind-body practice focused on flexibility, balance, and breathing." },
        };
        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
    }
}
