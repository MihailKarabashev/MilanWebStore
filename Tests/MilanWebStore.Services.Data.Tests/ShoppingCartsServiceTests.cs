namespace MilanWebStore.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using FluentAssertions;
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

    public class ShoppingCartsServiceTests
    {
        public ShoppingCartsServiceTests()
        {
            this.InitializeMapper();
        }

        [Theory]
        [InlineData("1", "username", 1, "Test", 1)]
        public async Task AddProductToShoppingCartAsyncShouldAddRightToDataBase(string id, string username, int productId, string productName, int quantity)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ShoppingCartProduct>(dbContext);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUser(username)).Returns(new ApplicationUser { Id = id, UserName = username, ShoppingCartId = 1 });

            var productsServiceMock = new Mock<IProductsService>();
            productsServiceMock.Setup(p => p.FindById(productId)).Returns(new Product { Id = productId, Name = productName });

            var service = new ShoppingCartsService(repository, usersServiceMock.Object, productsServiceMock.Object);

            await service.AddProductToShoppingCartAsync(productId, username, quantity);

            var count = await dbContext.ShoppingCartProducts.CountAsync();

            var actual = await dbContext.ShoppingCartProducts.FirstOrDefaultAsync();

            var expected = new ShoppingCartProduct
            {
                Product = new Product { Id = productId, Name = productName },
                ShoppingCartId = 1,
                Quantity = quantity,
            };

            Assert.Equal(1, count);
            expected.Product.Name.Should().BeEquivalentTo(actual.Product.Name);
            Assert.IsType<ShoppingCartProduct>(actual);
        }

        [Fact]
        public async Task AddProductToShoppingCartAsyncShouldThrowNullReferenceExceptionWhenProductIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ShoppingCartProduct>(dbContext);

            var productsServiceMock = new Mock<IProductsService>();
            productsServiceMock.Setup(p => p.FindById(2)).Returns(new Product { Id = 2, Name = "Test" });

            var service = new ShoppingCartsService(repository, null, productsServiceMock.Object);

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.AddProductToShoppingCartAsync(1, "1", 1));
            Assert.Equal(string.Format(ExceptionMessages.ProductNotFound, 1), exception.Message);
        }

        [Fact]
        public async Task AddProductToShoppingCartAsyncShouldThrowNullReferenceExceptionWhenUserIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ShoppingCartProduct>(dbContext);

            var productsServiceMock = new Mock<IProductsService>();
            productsServiceMock.Setup(p => p.FindById(1)).Returns(new Product { Id = 1, Name = "Test" });

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUser("Test")).Returns(new ApplicationUser { Id = "1", UserName = "Test", ShoppingCartId = 1 });

            var service = new ShoppingCartsService(repository, usersServiceMock.Object, productsServiceMock.Object);

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.AddProductToShoppingCartAsync(1, "SomeName", 1));
            Assert.Equal(string.Format(ExceptionMessages.UserNameNotFound, "SomeName"), exception.Message);
        }

        [Fact]
        public async Task AddProductToShoppingCartAsyncShouldThrowNullReferenceExceptionWhenQuantityIsLessOrEqualToNull()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ShoppingCartProduct>(dbContext);

            var productsServiceMock = new Mock<IProductsService>();
            productsServiceMock.Setup(p => p.FindById(1)).Returns(new Product { Id = 1, Name = "Test" });

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUser("Test")).Returns(new ApplicationUser { Id = "1", UserName = "Test", ShoppingCartId = 1 });

            var service = new ShoppingCartsService(repository, usersServiceMock.Object, productsServiceMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await service.AddProductToShoppingCartAsync(1, "Test", 0));
            Assert.Equal(string.Format(ExceptionMessages.InvalidQuantity), exception.Message);
        }

        [Fact]
        public async Task AnyProductsSouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);
            var user = new ApplicationUser { Id = "1", UserName = "Test" };
            var shoppingCart = new ShoppingCart { Id = 1 };
            shoppingCart.ApplicationUser = new ApplicationUser { Id = user.Id, UserName = user.UserName };

            var shoppingCartProduct = new ShoppingCartProduct
            {
                ShoppingCart = shoppingCart,
                ProductId = 1,
                Quantity = 1,
            };

            await dbContext.ShoppingCartProducts.AddAsync(shoppingCartProduct);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ShoppingCartProduct>(dbContext);

            var service = new ShoppingCartsService(repository, null, null);

            var anyProducts = service.AnyProducts(user.UserName);

            Assert.True(anyProducts);
        }

        [Theory]
        [InlineData("1", "username", 1, "Test", 1)]
        public async Task EditShoppingCartProductsAsyncShouldWorksCorrectly(string id, string username, int productId, string productName, int quantity)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ShoppingCartProduct>(dbContext);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUser(username)).Returns(new ApplicationUser { Id = id, UserName = username, ShoppingCartId = 1 });

            var productsServiceMock = new Mock<IProductsService>();
            productsServiceMock.Setup(p => p.FindById(productId)).Returns(new Product { Id = productId, Name = productName });

            var service = new ShoppingCartsService(repository, usersServiceMock.Object, productsServiceMock.Object);

            await service.AddProductToShoppingCartAsync(productId, username, quantity);

            await service.EditShoppingCartProductsAsync(1, username, 2);

            var actual = await dbContext.ShoppingCartProducts.FirstOrDefaultAsync();

            actual.Quantity.Should().Be(2);
            actual.Should().NotBeNull();
            actual.Id.Should().NotBe(2);
        }

        [Theory]
        [InlineData("1", "username", 1, "Test", 1)]
        public async Task EditShoppingCartProductsAsyncWhenShoppingCartProductIdIsNotFound(string id, string username, int productId, string productName, int quantity)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ShoppingCartProduct>(dbContext);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUser(username)).Returns(new ApplicationUser { Id = id, UserName = username, ShoppingCartId = 1 });

            var productsServiceMock = new Mock<IProductsService>();
            productsServiceMock.Setup(p => p.FindById(productId)).Returns(new Product { Id = productId, Name = productName });

            var service = new ShoppingCartsService(repository, usersServiceMock.Object, productsServiceMock.Object);

            await service.AddProductToShoppingCartAsync(productId, username, quantity);

            var exception = await Assert.
                ThrowsAnyAsync<NullReferenceException>
                (async () => await service.EditShoppingCartProductsAsync(2, username, quantity));
            exception.Message.Should().BeEquivalentTo(string.Format(ExceptionMessages.ShoppingCartProductIdNotFound, 2));
        }

        [Theory]
        [InlineData("1", "username", 1, "Test", 1)]
        public async Task EditShoppingCartProductsAsyncWhenShoppingCartWhenUserUsernameIsNotFound(string id, string username, int productId, string productName, int quantity)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ShoppingCartProduct>(dbContext);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUser(username)).Returns(new ApplicationUser { Id = id, UserName = username, ShoppingCartId = 1 });

            var productsServiceMock = new Mock<IProductsService>();
            productsServiceMock.Setup(p => p.FindById(productId)).Returns(new Product { Id = productId, Name = productName });

            var service = new ShoppingCartsService(repository, usersServiceMock.Object, productsServiceMock.Object);

            await service.AddProductToShoppingCartAsync(productId, username, quantity);

            var exception = await Assert.
                ThrowsAnyAsync<NullReferenceException>
                (async () => await service.EditShoppingCartProductsAsync(1, "FakeUserName", quantity));
            exception.Message.Should().BeEquivalentTo(string.Format(ExceptionMessages.UserNameNotFound, "FakeUserName"));
        }

        [Theory]
        [InlineData("1", "username", 1, "Test", 1)]
        public async Task EditShoppingCartProductsAsyncWhenShoppingCartWhenQuantityIsEqualOrLessThanZero(string id, string username, int productId, string productName, int quantity)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ShoppingCartProduct>(dbContext);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUser(username)).Returns(new ApplicationUser { Id = id, UserName = username, ShoppingCartId = 1 });

            var productsServiceMock = new Mock<IProductsService>();
            productsServiceMock.Setup(p => p.FindById(productId)).Returns(new Product { Id = productId, Name = productName });

            var service = new ShoppingCartsService(repository, usersServiceMock.Object, productsServiceMock.Object);

            await service.AddProductToShoppingCartAsync(productId, username, quantity);

            var exception = await Assert.
                ThrowsAnyAsync<InvalidOperationException>
                (async () => await service.EditShoppingCartProductsAsync(1, username, -1));
            exception.Message.Should().BeEquivalentTo(string.Format(ExceptionMessages.InvalidQuantity));
        }

        [Theory]
        [InlineData("1", "username", 1, "Test", 1)]
        public async Task DeleteShoppingCartProductAsyncShouldWorksCorrectly(string id, string username, int productId, string productName, int quantity)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ShoppingCartProduct>(dbContext);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUser(username)).Returns(new ApplicationUser { Id = id, UserName = username, ShoppingCartId = 1 });

            var productsServiceMock = new Mock<IProductsService>();
            productsServiceMock.Setup(p => p.FindById(productId)).Returns(new Product { Id = productId, Name = productName });

            var service = new ShoppingCartsService(repository, usersServiceMock.Object, productsServiceMock.Object);

            await service.AddProductToShoppingCartAsync(productId, username, quantity);

            await service.DeleteShoppingCartProductAsync(1, username);

            var count = await dbContext.ShoppingCartProducts.CountAsync();

            count.Should().Be(0);
            count.Should().NotBe(1);
        }

        [Theory]
        [InlineData("1", "username", 1, "Test", 1)]
        public async Task DeleteShoppingCartProductAsyncShouldThrowNullRefereceExceptionWhenShoppingCartProductIdIsNotFound(string id, string username, int productId, string productName, int quantity)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ShoppingCartProduct>(dbContext);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUser(username)).Returns(new ApplicationUser { Id = id, UserName = username, ShoppingCartId = 1 });

            var productsServiceMock = new Mock<IProductsService>();
            productsServiceMock.Setup(p => p.FindById(productId)).Returns(new Product { Id = productId, Name = productName });

            var service = new ShoppingCartsService(repository, usersServiceMock.Object, productsServiceMock.Object);

            await service.AddProductToShoppingCartAsync(productId, username, quantity);

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>
                (async () => await service.DeleteShoppingCartProductAsync(10, username));
            exception.Message.Should().BeEquivalentTo(string.Format(ExceptionMessages.ShoppingCartProductIdNotFound, 10));
        }

        [Theory]
        [InlineData("1", "username", 1, "Test", 1)]
        public async Task DeleteShoppingCartProductAsyncShouldThrowNullRefereceExceptionWhenUserUsernameIsNotFound(string id, string username, int productId, string productName, int quantity)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ShoppingCartProduct>(dbContext);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUser(username)).Returns(new ApplicationUser { Id = id, UserName = username, ShoppingCartId = 1 });

            var productsServiceMock = new Mock<IProductsService>();
            productsServiceMock.Setup(p => p.FindById(productId)).Returns(new Product { Id = productId, Name = productName });

            var service = new ShoppingCartsService(repository, usersServiceMock.Object, productsServiceMock.Object);

            await service.AddProductToShoppingCartAsync(productId, username, quantity);

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>
                (async () => await service.DeleteShoppingCartProductAsync(1, "FakeUserName"));
            exception.Message.Should().BeEquivalentTo(string.Format(ExceptionMessages.UserNameNotFound, "FakeUserName"));
        }

        [Theory]
        [InlineData("1", "username", 1, "Test", 1)]
        public async Task ClearShoppingCartProductsShouldThrowNullReferenceExceptionWhenUserUsernameIsNotFound(string id, string username, int productId, string productName, int quantity)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ShoppingCartProduct>(dbContext);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUser(username)).Returns(new ApplicationUser { Id = id, UserName = username, ShoppingCartId = 1 });

            var productsServiceMock = new Mock<IProductsService>();
            productsServiceMock.Setup(p => p.FindById(productId)).Returns(new Product { Id = productId, Name = productName });

            var service = new ShoppingCartsService(repository, usersServiceMock.Object, productsServiceMock.Object);

            await service.AddProductToShoppingCartAsync(productId, username, quantity);

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>
                (async () => await service.ClearShoppingCartAsync("FakeUserName"));
            exception.Message.Should().BeEquivalentTo(string.Format(ExceptionMessages.UserNameNotFound, "FakeUserName"));
        }

        [Theory]
        [InlineData("1", "username", 1, "Test", 1)]
        public async Task ClearShoppingCartProductsShouldWorksCorrectly(string id, string username, int productId, string productName, int quantity)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ShoppingCartProduct>(dbContext);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUser(username)).Returns(new ApplicationUser { Id = id, UserName = username, ShoppingCartId = 1 });

            var productsServiceMock = new Mock<IProductsService>();
            productsServiceMock.Setup(p => p.FindById(productId)).Returns(new Product { Id = productId, Name = productName });

            var shoppingCart = new ShoppingCart();
            shoppingCart.ApplicationUser = new ApplicationUser { Id = id, UserName = username };

            var shoppingCartProduct = new ShoppingCartProduct
            {
                ProductId = productId,
                ShoppingCart = shoppingCart,
                Quantity = quantity,
                IsDeleted = false,
            };

            await dbContext.ShoppingCartProducts.AddAsync(shoppingCartProduct);
            await dbContext.SaveChangesAsync();

            var service = new ShoppingCartsService(repository, usersServiceMock.Object, productsServiceMock.Object);

            await service.ClearShoppingCartAsync(username);

            var count = await dbContext.ShoppingCartProducts.CountAsync();

            count.Should().Be(0);
        }

        [Theory]
        [InlineData("1", "username", 1, "Test", 1)]
        public async Task GetAllShoppingCartProductsShouldWorksCorrectly(string id, string username, int productId, string productName, int quantity)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<ShoppingCartProduct>(dbContext);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUser(username)).Returns(new ApplicationUser { Id = id, UserName = username, ShoppingCartId = 1 });

            var productsServiceMock = new Mock<IProductsService>();
            productsServiceMock.Setup(p => p.FindById(productId)).Returns(new Product { Id = productId, Name = productName });

            var service = new ShoppingCartsService(repository, usersServiceMock.Object, productsServiceMock.Object);

            await service.AddProductToShoppingCartAsync(productId, username, quantity);

        }

        private void InitializeMapper() => AutoMapperConfig.
       RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
    }
}
