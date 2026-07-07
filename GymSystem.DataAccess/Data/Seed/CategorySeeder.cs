using GymSystem.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DataAccess.Data.Seed;

public static class CategorySeeder
{
    public static async Task SeedAsync(GymDbContext dbContext)
    {

        if (await dbContext.Categories.AnyAsync())
        {
            return;
        }

        var categories = new List<Category>
        {
            new (){ Name = "CrossFit"},
            new (){ Name = "Strength"},
            new (){ Name = "Boxing"},
            new (){ Name = "Dancing"},
            new (){ Name = "Yoga"},
        };
        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
    }
}
