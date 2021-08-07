namespace MilanWebStore.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Data.Models;

    public class ChildCategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.ChildCategories.AnyAsync())
            {
                return;
            }

            var childCategories = new List<ChildCategory>()
            {
                new ChildCategory { Name = "T-Shirts", CreatedOn = DateTime.UtcNow },
                new ChildCategory { Name = "Polos", CreatedOn = DateTime.UtcNow },
                new ChildCategory { Name = "Home Kit", CreatedOn = DateTime.UtcNow },
                new ChildCategory { Name = "Away Kit", CreatedOn = DateTime.UtcNow },
                new ChildCategory { Name = "Third Kit", CreatedOn = DateTime.UtcNow },
                new ChildCategory { Name = "Socks", CreatedOn = DateTime.UtcNow },
            };

            await dbContext.ChildCategories.AddRangeAsync(childCategories);
        }
    }
}
