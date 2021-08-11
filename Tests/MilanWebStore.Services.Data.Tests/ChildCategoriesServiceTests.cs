namespace MilanWebStore.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Common;
    using MilanWebStore.Data;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Data.Repositories;
    using MilanWebStore.Services.Mapping;
    using MilanWebStore.Web.ViewModels;
    using MilanWebStore.Web.ViewModels.ChildCategories;
    using Newtonsoft.Json;
    using Xunit;

    using Administration = MilanWebStore.Web.ViewModels.Administration.ChildCategory;

    public class ChildCategoriesServiceTests
    {
        public ChildCategoriesServiceTests()
        {
            this.InitializeMapper();
        }

        [Fact]
        public async Task CreateShouldAddRightToDataBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ChildCategory>(dbContext);

            var service = new ChildCategoriesService(repository);

            var viewModel = new Administration.ChildCategoryInputModel
            {
                Id = 1,
                Name = "T-Shirt",
            };

            await service.CreateAsync(viewModel);

            var expected = new ChildCategory()
            {
                Id = 1,
                Name = "T-Shirt",
            };

            var actual = await dbContext.ChildCategories.FirstOrDefaultAsync();
            var count = await dbContext.ChildCategories.CountAsync();

            Assert.IsType<ChildCategory>(actual);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Id , actual.Id);
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CreateShouldThrowAgrumentExceptionWhenChildCategoryAlreadyExsists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var childCategory = new ChildCategory()
            {
                Id = 1,
                Name = "T-Shirt",
            };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.ChildCategories.AddAsync(childCategory);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ChildCategory>(dbContext);

            var service = new ChildCategoriesService(repository);
            var input = new Administration.ChildCategoryInputModel { Id = 1, Name = "T-Shirt" };

            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateAsync(input));
            Assert.Equal(string.Format(ExceptionMessages.ChildCategoryAlreadyExist, input.Name), exception.Message);
        }

        [Fact]
        public async Task DeleteShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var childCategory = new ChildCategory { Id = 1, Name = "T-Shirt" };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.ChildCategories.AddAsync(childCategory);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ChildCategory>(dbContext);

            var service = new ChildCategoriesService(repository);

            await service.DeleteAsync(1);

            var actual = await dbContext.ChildCategories.CountAsync();

            Assert.Equal(0, actual);
        }

        [Fact]
        public async Task DeleteShouldThrowNullReferenceExceptionWhenTryToDeleteNotExsistingCategory()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var childCategory = new ChildCategory { Id = 1, Name = "T-Shirt" };

            using var dbContext = new ApplicationDbContext(options);
            await dbContext.ChildCategories.AddAsync(childCategory);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ChildCategory>(dbContext);

            var service = new ChildCategoriesService(repository);

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.DeleteAsync(2));
            Assert.Equal(string.Format(ExceptionMessages.ChildCategoryNotFound, 2), exception.Message);
        }

        [Fact]
        public async Task EditShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var childCategory = new ChildCategory { Id = 1, Name = "T-Shirt" };

            using var dbContext = new ApplicationDbContext(options);
            await dbContext.ChildCategories.AddAsync(childCategory);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ChildCategory>(dbContext);

            var service = new ChildCategoriesService(repository);

            var inputModel = new Administration.ChildCategoryViewModel { Id = 1, Name = "T-Shirt" };

            await service.EditAsync(inputModel);

            var actual = await dbContext.ChildCategories.FirstOrDefaultAsync();

            Assert.Equal(inputModel.Id, actual.Id);
            Assert.Equal(inputModel.Name, actual.Name);
        }

        [Fact]
        public async Task EditShouldThrowNullReferenceExceptionWhenChildCategoryIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var childCategory = new ChildCategory { Id = 1, Name = "T-Shirt" };

            using var dbContext = new ApplicationDbContext(options);
            await dbContext.ChildCategories.AddAsync(childCategory);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ChildCategory>(dbContext);

            var service = new ChildCategoriesService(repository);

            var inputModel = new Administration.ChildCategoryViewModel { Id = 2, Name = "T-Shirt" };

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.EditAsync(inputModel));
            Assert.Equal(string.Format(ExceptionMessages.ChildCategoryNotFound, inputModel.Id), exception.Message);
        }

        [Fact]
        public async Task GetAllSChildCategoriesShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var childCategories = new List<ChildCategory>
            {
                new ChildCategory() { Id = 1, Name = "Polos" },
                new ChildCategory() { Id = 2, Name = "T-Shirt" },
                new ChildCategory() { Id = 3, Name = "Kits" },
            };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.ChildCategories.AddRangeAsync(childCategories);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ChildCategory>(dbContext);

            var service = new ChildCategoriesService(repository);

            var result = service.GetAll<ChildCategoryViewModel>();

            var count = result.Count();

            Assert.IsType<List<ChildCategoryViewModel>>(result);
            Assert.Equal(3, count);
        }

        [Fact]
        public async Task GetByIdShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var childCategory = new ChildCategory { Id = 1, Name = "T-Shirt" };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.ChildCategories.AddAsync(childCategory);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ChildCategory>(dbContext);

            var service = new ChildCategoriesService(repository);

            var expected = new ChildCategoryViewModel()
            {
                Id = 1,
                Name = "T-Shirt",
            };

            var actual = service.GetById<ChildCategoryViewModel>(expected.Id);

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact]
        public async Task GetAllKitsShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var childCategories = new List<ChildCategory>
            {
                new ChildCategory() { Id = 1, Name = "Polos" },
                new ChildCategory() { Id = 2, Name = "T-Shirt" },
                new ChildCategory() { Id = 3, Name = "Kit" },
            };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.ChildCategories.AddRangeAsync(childCategories);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ChildCategory>(dbContext);

            var service = new ChildCategoriesService(repository);

            var result = service.GetAllKits<ChildCategoryViewModel>();

            var count = result.Count();

            Assert.IsType<List<ChildCategoryViewModel>>(result);
            Assert.Equal(1, count);
        }
        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
    }
}
