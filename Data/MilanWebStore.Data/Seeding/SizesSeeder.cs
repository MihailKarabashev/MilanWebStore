namespace MilanWebStore.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Data.Models;

    public class SizesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Sizes.AnyAsync())
            {
                return;
            }

            var sizes = new List<Size>()
            {
                new Size { Name = "S", CreatedOn = DateTime.UtcNow },
                new Size { Name = "M", CreatedOn = DateTime.UtcNow },
                new Size { Name = "L", CreatedOn = DateTime.UtcNow },
                new Size { Name = "XL", CreatedOn = DateTime.UtcNow },
            };

            await dbContext.AddRangeAsync(sizes);
        }
    }
}
