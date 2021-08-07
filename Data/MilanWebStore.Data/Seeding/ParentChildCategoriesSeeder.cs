namespace MilanWebStore.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Data.Models;

    public class ParentChildCategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.ParentChildCategories.AnyAsync())
            {
                return;
            }

            var parentCategories = await dbContext.ParentCategories
                .Select(x => x.Id).ToListAsync();

            var childCategories = await dbContext.ChildCategories
                .Select(x => x.Id).ToListAsync();

            var parentChildCategories = new List<ParentChildCategory>();

            foreach (var parentCategoryId in parentCategories)
            {
                foreach (var childCategoryId in childCategories)
                {
                    parentChildCategories.Add(new ParentChildCategory
                    {
                        ParentCateogryId = parentCategoryId,
                        ChildCategoryId = childCategoryId,
                    });
                }
            }

            await dbContext.ParentChildCategories.AddRangeAsync(parentChildCategories);
        }
    }
}
