namespace MilanWebStore.Services.Data.Tests
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Common;
    using MilanWebStore.Data;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Data.Repositories;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Services.Mapping;
    using MilanWebStore.Web.ViewModels;
    using Moq;
    using Xunit;

    public class FavoriteProductsServiceTests
    {

        public FavoriteProductsServiceTests()
        {
            this.InitializeMapper();
        }

        [Theory]
        [InlineData("1", "Test", 1, "ProductTest")]
        public async Task AddAsyncShouldAddRightToDataBase(string userId, string username, int productId, string productName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDataBase").Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<FavoriteProduct>(dbContext);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUserById(userId)).Returns(new ApplicationUser() { Id = userId, UserName = username });

            var productServiceMock = new Mock<IProductsService>();
            productServiceMock.Setup(p => p.FindById(productId)).Returns(new Product() { Id = productId, Name = productName });

            var service = new FavoriteProductsService(repository, productServiceMock.Object, usersServiceMock.Object);

            var expected = new FavoriteProduct()
            {
                Id = 1,
                ApplicationUserId = userId,
                ProductId = productId,
            };

            await service.AddAsync(userId, productId);

            var actual = await dbContext.FavoriteProducts
                .FirstOrDefaultAsync();

            var count = await dbContext.FavoriteProducts.CountAsync();

            Assert.Equal(1, count);
            Assert.IsType<FavoriteProduct>(actual);
            Assert.Equal(expected.ApplicationUserId, actual.ApplicationUserId);
            Assert.Equal(expected.ProductId, actual.ProductId);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Theory]
        [InlineData("1", 1, "ProductTest")]
        public async Task AddAsyncShouldThrowNullReferenceExceptionIfProductIsNotFound(string userId, int productId, string productName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<FavoriteProduct>(dbContext);


            var productServiceMock = new Mock<IProductsService>();
            productServiceMock.Setup(p => p.FindById(productId)).Returns(new Product() { Id = productId, Name = productName });

            var service = new FavoriteProductsService(repository, productServiceMock.Object, null);

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.AddAsync(userId, 2));
            Assert.Equal(string.Format(ExceptionMessages.ProductNotFound, 2), exception.Message);
        }

        [Theory]
        [InlineData("1", "Test", 1, "ProductTest")]
        public async Task AddAsyncShouldThrowNullReferenceExceptionIfUserIsNotFound(string userId, string username, int productId, string productName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<FavoriteProduct>(dbContext);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUserById(userId)).Returns(new ApplicationUser() { Id = userId, UserName = username });

            var productServiceMock = new Mock<IProductsService>();
            productServiceMock.Setup(p => p.FindById(productId)).Returns(new Product() { Id = productId, Name = productName });

            var service = new FavoriteProductsService(repository, productServiceMock.Object, usersServiceMock.Object);

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.AddAsync("2", productId));
            Assert.Equal(string.Format(ExceptionMessages.UserIdNotFound, "2"), exception.Message);
        }

        [Fact]
        public async Task DeleteAsyncShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var favoriteProduct = new FavoriteProduct { Id = 1, ApplicationUserId = "1", ProductId = 1 };
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.FavoriteProducts.AddAsync(favoriteProduct);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<FavoriteProduct>(dbContext);

            var service = new FavoriteProductsService(repository, null, null);

            await service.DeleteAsync(1);

            var count = await dbContext.FavoriteProducts.CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task DeleteAsyncShouldThrowNullReferenceExceptionIfFavoriteProductsIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var favoriteProduct = new FavoriteProduct { Id = 1, ApplicationUserId = "1", ProductId = 1 };
            using var dbContext = new ApplicationDbContext(options);
            await dbContext.FavoriteProducts.AddAsync(favoriteProduct);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<FavoriteProduct>(dbContext);

            var service = new FavoriteProductsService(repository, null, null);

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.DeleteAsync(2));
            Assert.Equal(string.Format(ExceptionMessages.FavoriteProductIdNotFound, 2), exception.Message);
        }

        private void InitializeMapper() => AutoMapperConfig.
           RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
    }
}
