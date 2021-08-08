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
    using MilanWebStore.Data.Common.Repositories;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Data.Repositories;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Services.Mapping;
    using MilanWebStore.Web.ViewModels;
    using MilanWebStore.Web.ViewModels.Administration.Sizes;
    using MilanWebStore.Web.ViewModels.Sizes;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class SizesServiceTest
    {

        public SizesServiceTest()
        {
            this.InitializeMapper();
        }

        [Fact]
        public async Task CreateShouldAddRightToDataBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);


            using var repository = new EfDeletableEntityRepository<Size>(dbContext);

            var service = new SizesService(repository);

            await service.CreateAsync(new SizeInputModel { Name = "M" });

            var expected = new Size()
            {
                Id = 1,
                Name = "M",
                IsDeleted = false,
            };

            var actual = await dbContext.Sizes.FirstOrDefaultAsync();
            var count = await dbContext.Sizes.CountAsync();

            Assert.IsType<Size>(actual);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task EditShouldThrowExceptionWhenSizeIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var size = new Size()
            {
                Id = 1,
                Name = "M",
            };

            using var dbContext = new ApplicationDbContext(options);

            dbContext.Sizes.Add(size);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Size>(dbContext);

            var service = new SizesService(repository);

            var editSizeModel = new EditSizeViewModel()
            {
                Id = 2,
                Name = "S",
            };

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.EditAsync(editSizeModel));
            Assert.Equal(string.Format(ExceptionMessages.SizeIdNotFoud, editSizeModel.Id), exception.Message);
        }

        [Fact]
        public async Task EditShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var size = new Size()
            {
                Id = 1,
                Name = "S",
            };

            using var dbContext = new ApplicationDbContext(options);

            dbContext.Sizes.Add(size);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Size>(dbContext);

            var service = new SizesService(repository);

            var editSizeModel = new EditSizeViewModel()
            {
                Id = 1,
                Name = "M",
            };

            await service.EditAsync(editSizeModel);
            var actual = await dbContext.Sizes.FirstOrDefaultAsync();

            Assert.Equal(editSizeModel.Id, actual.Id);
            Assert.Equal(editSizeModel.Name, actual.Name);
        }

        [Fact]
        public async Task DeleteShouldThrowExceptionWhenSizeIdNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var size = new Size()
            {
                Id = 1,
                Name = "S",
            };

            using var dbContext = new ApplicationDbContext(options);
            dbContext.Sizes.Add(size);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Size>(dbContext);

            var service = new SizesService(repository);

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.DeleteAsync(2));
            Assert.Equal(string.Format(ExceptionMessages.SizeIdNotFoud, 2), exception.Message);
        }

        [Fact]
        public async Task DeleteShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var size = new Size()
            {
                Id = 1,
                Name = "S",
            };

            using var dbContext = new ApplicationDbContext(options);

            dbContext.Sizes.Add(size);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Size>(dbContext);

            var service = new SizesService(repository);

            await service.DeleteAsync(1);

            var actual = await dbContext.Sizes.CountAsync();

            Assert.Equal(0, actual);
        }

        [Fact]
        public async Task GetAllSizesShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var sizes = new List<Size>
            {
                new Size() { Id = 1, Name = "S" },
                new Size() { Id = 2, Name = "M" },
                new Size() { Id = 3, Name = "L" },
                new Size() { Id = 4, Name = "XL" },
            };

            using var dbContext = new ApplicationDbContext(options);

            dbContext.Sizes.AddRange(sizes);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Size>(dbContext);

            var service = new SizesService(repository);

            var result = service.GetAll<SizeViewModel>();

            var count = result.Count();

            Assert.Equal(4, count);
        }

        [Fact]
        public async Task GetByIdShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var size = new Size { Id = 1, Name = "S" };
            using var dbContext = new ApplicationDbContext(options);

            dbContext.Sizes.Add(size);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Size>(dbContext);

            var service = new SizesService(repository);

            var expected = new SizeViewModel()
            {
                Id = 1,
                Name = "S",
            };

            var actual = service.GetById<SizeViewModel>(1);

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact]
        public async Task GetByIdShouldThrowExceptionWhenSizeIsNotFoundById()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var size = new Size { Id = 1, Name = "S" };
            using var dbContext = new ApplicationDbContext(options);

            dbContext.Sizes.Add(size);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Size>(dbContext);

            var service = new SizesService(repository);

            var exception = Assert.Throws<NullReferenceException>(() => service.GetById<SizeViewModel>(2));
            Assert.Equal(string.Format(ExceptionMessages.SizeIdNotFoud, 2), exception.Message);
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
    }
}
