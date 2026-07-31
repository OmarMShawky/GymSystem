using GymSystem.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GymSystem.DataAccess.Data.Seed;

public static class IdentitySeeder
{
    public const string SuperAdminRole = "SuperAdmin";
    public const string AdminRole = "Admin";

    private const string _seedAdminEmail = "superadmin@powerfitness.com";
    private const string _seedAdminPassword = "SuperAdmin@123";

    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        await SeedRolesAsync(roleManager);
        await SeedSuperAdminAsync(userManager);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in new[] { SuperAdminRole, AdminRole })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager)
    {
        var existing = await userManager.FindByEmailAsync(_seedAdminEmail);

        if (existing is null)
        {
            var user = new ApplicationUser
            {
                UserName = _seedAdminEmail,
                Email = _seedAdminEmail,
                EmailConfirmed = true,
                FirstName = "Super",
                LastName = "Admin"
            };

            var created = await userManager.CreateAsync(user, _seedAdminPassword);

            if (!created.Succeeded)
            {
                var errors = string.Join("; ", created.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Could not seed the admin user: {errors}");
            }

            existing = user;
        }

        if (!await userManager.IsInRoleAsync(existing, SuperAdminRole))
        {
            await userManager.AddToRoleAsync(existing, SuperAdminRole);
        }
    }
}
