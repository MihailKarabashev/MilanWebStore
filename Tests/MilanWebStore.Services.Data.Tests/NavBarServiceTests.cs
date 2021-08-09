namespace MilanWebStore.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Data;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Data.Repositories;
    using MilanWebStore.Services.Mapping;
    using MilanWebStore.Web.ViewModels;
    using MilanWebStore.Web.ViewModels.Administration.ParentCategory;
    using Xunit;

    public class NavBarServiceTests
    {
        public NavBarServiceTests()
        {
            this.InitializeMapper();
        }

        [Fact]
        public async Task NavBarGetAllParentAndChildCategoriesManyToManyShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var parentCategory = new List<ParentCategory>
            {
                new ParentCategory() { Id = 1, Name = "Men" },
                new ParentCategory() { Id = 2, Name = "Women" },
            };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.ParentCategories.AddRangeAsync(parentCategory);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ParentCategory>(dbContext);

            var service = new NavBarService(repository);

            var result = service.GetAllParentChildCategories<ParentCategoryViewModel>();

            var count = result.Count();

            Assert.Equal(2, count);
        }

        private void InitializeMapper() => AutoMapperConfig.
           RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
    }
}
