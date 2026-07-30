using System.Text.Json;

namespace GymSystem.DataAccess.Data.Seed;

public static class PlanSeeder
{
    private static readonly string _plansFilePath =
        Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "plans.json");

    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task SeedAsync(GymDbContext dbContext)
    {
        await dbContext.Database.MigrateAsync();

        if (await dbContext.Plans.AnyAsync())
            return;

        var plans = await ReadPlansAsync();

        if (plans.Count == 0)
            return;

        await dbContext.Plans.AddRangeAsync(plans);
        await dbContext.SaveChangesAsync();
    }

    private static async Task<List<Plan>> ReadPlansAsync()
    {
        if (!File.Exists(_plansFilePath))
        {
            throw new FileNotFoundException(
                $"Plan seed data was not found at '{_plansFilePath}'. " +
                "Ensure plans.json is set to copy to the output directory.",
                _plansFilePath);
        }

        await using var stream = File.OpenRead(_plansFilePath);

        var plans = await JsonSerializer.DeserializeAsync<List<Plan>>(stream, _serializerOptions);

        return plans ?? [];
    }
}
