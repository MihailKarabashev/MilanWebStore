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
    using Newtonsoft.Json;
    using Xunit;

    using Administration = MilanWebStore.Web.ViewModels.Administration.ParentCategory;

    public class ParentCategoriesServiceTests
    {
        public ParentCategoriesServiceTests()
        {
            this.InitializeMapper();
        }

        [Fact]
        public async Task CreateShouldAddRightToDataBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ParentCategory>(dbContext);

            var service = new ParentCategoriesService(repository);

            var viewModel = new Administration.ParentCategoryInputModel { Id = 1, Name = "Men" };

            await service.CreateAsync(viewModel);

            var expected = new ParentCategory()
            {
                Id = 1,
                Name = "Men",
            };

            var actual = await dbContext.ParentCategories.FirstOrDefaultAsync();
            var count = await dbContext.ParentCategories.CountAsync();

            Assert.IsType<ParentCategory>(actual);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CreateShouldThrowAgrumentExceptionWhenParentCategoryAlreadyExsists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var parentCategory = new ParentCategory() { Id = 1, Name = "Men" };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.ParentCategories.AddAsync(parentCategory);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ParentCategory>(dbContext);

            var service = new ParentCategoriesService(repository);

            var input = new Administration.ParentCategoryInputModel { Id = 1, Name = "Men" };

            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateAsync(input));
            Assert.Equal(string.Format(ExceptionMessages.ParentCategoryAlreadyExist, input.Name), exception.Message);
        }

        [Fact]
        public async Task DeleteShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var parentCategory = new ParentCategory { Id = 1, Name = "Men" };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.ParentCategories.AddAsync(parentCategory);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ParentCategory>(dbContext);

            var service = new ParentCategoriesService(repository);

            await service.DeleteAsync(1);

            var actual = await dbContext.ParentCategories.CountAsync();

            Assert.Equal(0, actual);
        }

        [Fact]
        public async Task DeleteShouldThrowNullReferenceExceptionWhenTryToDeleteNotExsistingParentCategory()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var parentCategory = new ParentCategory { Id = 1, Name = "Men" };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.ParentCategories.AddAsync(parentCategory);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ParentCategory>(dbContext);

            var service = new ParentCategoriesService(repository);

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.DeleteAsync(2));
            Assert.Equal(string.Format(ExceptionMessages.ParentCategoryNotFound, 2), exception.Message);
        }

        [Fact]
        public async Task EditShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var parentCategory = new ParentCategory { Id = 1, Name = "Men" };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.ParentCategories.AddAsync(parentCategory);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ParentCategory>(dbContext);

            var service = new ParentCategoriesService(repository);

            var inputModel = new Administration.ParentCategoryViewModel { Id = 1, Name = "Men" };

            await service.EditAsync(inputModel);

            var actual = await dbContext.ParentCategories.FirstOrDefaultAsync();

            Assert.Equal(inputModel.Id, actual.Id);
            Assert.Equal(inputModel.Name, actual.Name);
        }

        [Fact]
        public async Task EditShouldThrowNullReferenceExceptionWhenParentCategoryIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var parentCategory = new ParentCategory { Id = 1, Name = "Men" };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.ParentCategories.AddAsync(parentCategory);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ParentCategory>(dbContext);

            var service = new ParentCategoriesService(repository);

            var inputModel = new Administration.ParentCategoryViewModel { Id = 2, Name = "Women" };

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.EditAsync(inputModel));
            Assert.Equal(string.Format(ExceptionMessages.ParentCategoryNotFound, inputModel.Id), exception.Message);
        }

        [Fact]
        public async Task GetAllSChildCategoriesShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var parentCategories = new List<ParentCategory>
            {
                new ParentCategory() { Id = 1, Name = "Men" },
                new ParentCategory() { Id = 2, Name = "Women" },
                new ParentCategory() { Id = 3, Name = "Kids" },
            };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.ParentCategories.AddRangeAsync(parentCategories);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ParentCategory>(dbContext);

            var service = new ParentCategoriesService(repository);

            var result = service.GetAll<Administration.ParentCategoryViewModel>();

            var count = result.Count();

            Assert.IsType<List<Administration.ParentCategoryViewModel>>(result);
            Assert.Equal(3, count);
        }

        [Fact]
        public async Task GetByIdShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var parentCategory = new ParentCategory { Id = 1, Name = "Men" };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.ParentCategories.AddAsync(parentCategory);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ParentCategory>(dbContext);

            var service = new ParentCategoriesService(repository);

            var expected = new Administration.ParentCategoryViewModel()
            {
                Id = 1,
                Name = "Men",
            };

            var actual = service.GetById<Administration.ParentCategoryViewModel>(expected.Id);

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        private void InitializeMapper() => AutoMapperConfig.
           RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
    }
}
