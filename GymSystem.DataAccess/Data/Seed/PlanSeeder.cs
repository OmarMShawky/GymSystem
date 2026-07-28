namespace GymSystem.DataAccess.Data.Seed;

public static class PlanSeeder
{
    public static async Task SeedAsync(GymDbContext dbContext)
    {
        await dbContext.Database.MigrateAsync();

        if (await dbContext.Plans.AnyAsync())
        {
            return;
        }

        var plans = new List<Plan>
        {
            new() {
                Name = "One Day",
                Description = "One day access to gym equipment and basic facilities.",
                DurationDays = 1,
                Price = 150,
                IsActive = true,
                CreatedAt = DateTime.Now },

            new() {
                Name = "Basic Monthly",
                Description = "Monthly access to gym equipment during working hours.",
                DurationDays = 30,
                Price = 750,
                IsActive = true,
                CreatedAt = DateTime.Now },

            new() {
                Name = "Standard Quarterly",
                Description = "Three months access with better value for regular members.",
                DurationDays = 90,
                Price = 2000,
                IsActive = true,
                CreatedAt = DateTime.Now },
            new() {
                Name = "Premium Annual",
                Description = "Full year membership with premium access and best value.",
                DurationDays = 365,
                Price = 7000,
                IsActive = true,
                CreatedAt = DateTime.Now },
            new() {
                Name = "Old Summer Offer",
                Description = "Inactive old offer kept for historical membership records.",
                DurationDays = 60,
                Price = 1200,
                IsActive = false,
                CreatedAt = DateTime.Now }
        };

        await dbContext.Plans.AddRangeAsync(plans);
        await dbContext.SaveChangesAsync();
    }
}
