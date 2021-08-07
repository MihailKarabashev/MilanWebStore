namespace MilanWebStore.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Data.Models;

    public class ParentCategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.ParentCategories.AnyAsync())
            {
                return;
            }

            var parentCategories = new List<ParentCategory>()
            {
                new ParentCategory { Name = "Men", CreatedOn = DateTime.UtcNow,},
                new ParentCategory { Name = "Women", CreatedOn = DateTime.UtcNow },
                new ParentCategory { Name = "Kids", CreatedOn = DateTime.UtcNow },
            };

            await dbContext.ParentCategories.AddRangeAsync(parentCategories);
        }
    }
}
