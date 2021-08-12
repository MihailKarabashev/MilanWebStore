namespace MilanWebStore.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Common;
    using MilanWebStore.Data;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Data.Models.Enums;
    using MilanWebStore.Data.Repositories;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Services.Mapping;
    using MilanWebStore.Web.ViewModels;
    using MilanWebStore.Web.ViewModels.Orders;
    using MilanWebStore.Web.ViewModels.ShoppingCarts;
    using Moq;
    using Xunit;

    public class OrdersServiceTests
    {
        public OrdersServiceTests()
        {
            this.InitializeMapper();
        }

        [Fact]
        public async Task FindOrderByIdShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            var orders = new List<Order>
            {
                new Order { Id = 1 , OrderStatus = OrderStatus.Pending},
                new Order { Id = 2 , OrderStatus = OrderStatus.NotDelivered},
                new Order { Id = 3 , OrderStatus = OrderStatus.Delivered},
            };

            await dbContext.Orders.AddRangeAsync(orders);
            await dbContext.SaveChangesAsync();

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, null, null, null, null);

            var order = service.FindOrderById(3);

            order.Id.Should().Be(3);
            order.OrderStatus.Should().BeEquivalentTo(OrderStatus.Delivered);
        }

        [Fact]
        public async Task GetOrderByIdShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            var user = new ApplicationUser() { Id = "1", UserName = "username" };
            var address = new Address { Id = 1, Street = "Street", City = "City", ZipCode = "ZipCode" };

            var order = this.SeedOrderDetails(1, OrderStatus.Pending, ShippingMethod.Home, PaymentMethod.CashОnDelivery, 266, user, address);

            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, null, null, null, null);

            var viewModel = service.GetOrderById<MyOrderViewModel>(1);

            var count = await dbContext.Orders.CountAsync();

            count.Should().Be(1);
            order.Id.Should().Be(1);
            order.OrderStatus.Should().BeEquivalentTo(OrderStatus.Pending);
        }

        [Fact]
        public async Task GetUnProcessedOrdersShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            var user = new ApplicationUser() { Id = "1", UserName = "username" };
            var address = new Address { Id = 1, Street = "Street", City = "City", ZipCode = "ZipCode" };

            var list = new List<Order>()
            {
                this.SeedOrderDetails(1,OrderStatus.NotDelivered,ShippingMethod.Home,PaymentMethod.CashОnDelivery,266,user,address),
                this.SeedOrderDetails(2,OrderStatus.NotDelivered,ShippingMethod.Home,PaymentMethod.CashОnDelivery,555,user,address),
                this.SeedOrderDetails(3,OrderStatus.Pending,ShippingMethod.Office,PaymentMethod.CashОnDelivery,111,user,address),
            };

            await dbContext.Orders.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, null, null, null, null);

            var orders = service.GetUnProcessedOrders<MyOrderViewModel>();

            orders.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetProcessedOrdersShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            var user = new ApplicationUser() { Id = "1", UserName = "username" };
            var address = new Address { Id = 1, Street = "Street", City = "City", ZipCode = "ZipCode" };

            var list = new List<Order>()
            {
                this.SeedOrderDetails(1,OrderStatus.NotDelivered,ShippingMethod.Home,PaymentMethod.CashОnDelivery,266,user,address),
                this.SeedOrderDetails(2,OrderStatus.Pending,ShippingMethod.Home,PaymentMethod.CashОnDelivery,555,user,address),
                this.SeedOrderDetails(3,OrderStatus.Pending,ShippingMethod.Office,PaymentMethod.CashОnDelivery,111,user,address),
            };

            await dbContext.Orders.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, null, null, null, null);

            var orders = service.GetProcessedOrders<MyOrderViewModel>();

            orders.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetDeliveredOrdersShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            var user = new ApplicationUser() { Id = "1", UserName = "username" };
            var address = new Address { Id = 1, Street = "Street", City = "City", ZipCode = "ZipCode" };

            var list = new List<Order>()
            {
                this.SeedOrderDetails(1,OrderStatus.NotDelivered,ShippingMethod.Home,PaymentMethod.CashОnDelivery,266,user,address),
                this.SeedOrderDetails(2,OrderStatus.Pending,ShippingMethod.Home,PaymentMethod.CashОnDelivery,555,user,address),
                this.SeedOrderDetails(3,OrderStatus.Delivered,ShippingMethod.Office,PaymentMethod.CashОnDelivery,111,user,address),
            };

            await dbContext.Orders.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, null, null, null, null);

            var orders = service.GetDeliveredOrders<MyOrderViewModel>();

            orders.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetProcessedOrderShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            var user = new ApplicationUser() { Id = "1", UserName = "username" };
            var address = new Address { Id = 1, Street = "Street", City = "City", ZipCode = "ZipCode" };

            var list = new List<Order>()
            {
                this.SeedOrderDetails(1,OrderStatus.NotDelivered,ShippingMethod.Home,PaymentMethod.CashОnDelivery,266,user,address),
                this.SeedOrderDetails(2,OrderStatus.Pending,ShippingMethod.Home,PaymentMethod.CashОnDelivery,555,user,address),
                this.SeedOrderDetails(3,OrderStatus.Delivered,ShippingMethod.Office,PaymentMethod.CashОnDelivery,111,user,address),
            };

            await dbContext.Orders.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, null, null, null, null);

            var order = service.GetProcessedOrder<MyOrderViewModel>(user.UserName);

            order.Id.Should().Be(2);
        }

        [Fact]
        public async Task ProcessOrderMethodShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            var user = new ApplicationUser() { Id = "1", UserName = "username" };
            var address = new Address { Id = 1, Street = "Street", City = "City", ZipCode = "ZipCode" };

            var list = new List<Order>()
            {
                this.SeedOrderDetails(1,OrderStatus.NotDelivered,ShippingMethod.Home,PaymentMethod.CashОnDelivery,266,user,address),
                this.SeedOrderDetails(2,OrderStatus.Pending,ShippingMethod.Home,PaymentMethod.CashОnDelivery,555,user,address),
                this.SeedOrderDetails(3,OrderStatus.NotDelivered,ShippingMethod.Office,PaymentMethod.CashОnDelivery,111,user,address),
            };

            await dbContext.Orders.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, null, null, null, null);

            await service.ProcessOrder(1);

            var proccessOrder = await dbContext.Orders.Where(x => x.Id == 1).FirstOrDefaultAsync();

            proccessOrder.Id.Should().Be(1);
            proccessOrder.OrderStatus.Should().BeEquivalentTo(OrderStatus.Pending);
        }

        [Fact]
        public async Task ProcessOrderMethodShouldThrowNullArgumentExceptionWhenOrderIdIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, null, null, null, null);

            var exception = await Assert.ThrowsAsync<NullReferenceException>
                (async () => await service.ProcessOrder(1));

            Assert.Equal(string.Format(ExceptionMessages.OrderIdNotFound, 1), exception.Message);
        }

        [Fact]
        public async Task DeliverOrderMethodShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            var user = new ApplicationUser() { Id = "1", UserName = "username" };
            var address = new Address { Id = 1, Street = "Street", City = "City", ZipCode = "ZipCode" };

            var list = new List<Order>()
            {
                this.SeedOrderDetails(1,OrderStatus.NotDelivered,ShippingMethod.Home,PaymentMethod.CashОnDelivery,266,user,address),
                this.SeedOrderDetails(2,OrderStatus.Pending,ShippingMethod.Home,PaymentMethod.CashОnDelivery,555,user,address),
                this.SeedOrderDetails(3,OrderStatus.Pending,ShippingMethod.Office,PaymentMethod.CashОnDelivery,111,user,address),
            };

            await dbContext.Orders.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, null, null, null, null);

            await service.DeliverOrder(2);

            var proccessOrder = await dbContext.Orders.Where(x => x.Id == 2).FirstOrDefaultAsync();

            proccessOrder.Id.Should().Be(2);
            proccessOrder.OrderStatus.Should().BeEquivalentTo(OrderStatus.Delivered);
        }

        [Fact]
        public async Task DeliverOrderMethodShouldThrowNullArgumentExceptionWhenOrderIdIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, null, null, null, null);

            var exception = await Assert.ThrowsAsync<NullReferenceException>
                (async () => await service.DeliverOrder(1));

            Assert.Equal(string.Format(ExceptionMessages.OrderIdNotFound, 1), exception.Message);
        }

        [Fact]
        public async Task GetUserOrdersShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            var user = new ApplicationUser() { Id = "1", UserName = "username" };
            var address = new Address { Id = 1, Street = "Street", City = "City", ZipCode = "ZipCode" };

            var list = new List<Order>()
            {
                this.SeedOrderDetails(1,OrderStatus.NotDelivered,ShippingMethod.Home,PaymentMethod.CashОnDelivery,266,user,address),
                this.SeedOrderDetails(2,OrderStatus.Pending,ShippingMethod.Home,PaymentMethod.CashОnDelivery,555,user,address),
                this.SeedOrderDetails(3,OrderStatus.Delivered,ShippingMethod.Office,PaymentMethod.CashОnDelivery,111,user,address),
            };

            await dbContext.Orders.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, null, null, null, null);

            var orders = service.GetUserOrders<MyOrderViewModel>(user.UserName);

            orders.Count().Should().Be(2);
        }

        [Fact]
        public async Task UpdatePaymentStatusShouldUpdateCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            var user = new ApplicationUser() { Id = "1", UserName = "username" };
            var address = new Address { Id = 1, Street = "Street", City = "City", ZipCode = "ZipCode" };

            var list = new List<Order>()
            {
                new Order() { Id = 1, PaymentStatus = PaymentStatus.UnPaid },
                new Order() { Id = 2, PaymentStatus = PaymentStatus.Paid },
            };

            await dbContext.Orders.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, null, null, null, null);

            await service.UpdatePaymentStatus(1);

            var order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == 1);

            order.PaymentStatus.Should().Be(PaymentStatus.Paid);
        }

        [Fact]
        public async Task UpdatePaymentStatusThrowNullReferenceExceptionWhenOrderIdIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, null, null, null, null);

            var exception = await Assert.ThrowsAsync<NullReferenceException>
                (async () => await service.UpdatePaymentStatus(1));

            Assert.Equal(string.Format(ExceptionMessages.OrderIdNotFound, 1), exception.Message);
        }

        [Fact]
        public async Task GetAllOrderProductsByIdShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            var image = new Image() { Id = "ss1id", Extention = "jpg" };
            var product = new Product { Id = 1, Name = "Name", Price = 266, };
            product.Images.Add(image);
            var order = new Order() { Id = 1 };

            var oderProduct = new OrderProduct { Order = order, Product = product, Quantity = 2 };

            await dbContext.OrderProducts.AddAsync(oderProduct);
            await dbContext.SaveChangesAsync();

            using var ordersRepository = new EfRepository<OrderProduct>(dbContext);

            var service = new OrdersService(null, null, null, ordersRepository, null, null);

            var orders = service.GetAllOrderProductsById<OrderProductViewModel>(1);

            orders.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetAllOrderProductTotalPriceShouldReturnTotalPriceOfAllProductsForSingleOrder()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            var product = new Product { Id = 1, Name = "Name", Price = 200, };
            var secoundProduct = new Product() { Id = 2, Name = "SecoundName", Price = 100 };
            var order = new Order() { Id = 1 };

            var list = new List<OrderProduct>
            {
                new OrderProduct { Order = order, Product = product, Quantity = 1 },
                new OrderProduct { Order = order, Product = secoundProduct, Quantity = 1},
            };

            await dbContext.OrderProducts.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            using var ordersRepository = new EfRepository<OrderProduct>(dbContext);

            var service = new OrdersService(null, null, null, ordersRepository, null, null);

            var actual = service.GetAllOrderProductTotalPrice(1);

            Assert.Equal(300, actual);
        }

        [Fact]
        public async Task CompleteOrderAsyncShouldAddRightToDataBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);
            var user = new ApplicationUser { Id = "ssd1Id", Email = "username", UserName = "username" };
            var order = new Order { Id = 1, ApplicationUser = user, OrderStatus = OrderStatus.Pending };
            var product = new Product { Id = 1, Name = "Name" };
            var shoppingCart = new ShoppingCart { Id = 1, ApplicationUser = user };

            await dbContext.Products.AddAsync(product);
            await dbContext.Orders.AddAsync(order);
            await dbContext.ShoppingCarts.AddAsync(shoppingCart);

            await dbContext.SaveChangesAsync();

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);
            using var orderProductsRepository = new EfRepository<OrderProduct>(dbContext);
            using var productsRepository = new EfDeletableEntityRepository<Product>(dbContext);

            var shoppingCartsServiceMock = new Mock<IShoppingCartsService>();
            shoppingCartsServiceMock.Setup(x => x.GetAllShoppingCartProducts("username"))
              .Returns(new List<ShoppingCartProductsViewModel>
             {
                      new ShoppingCartProductsViewModel() { Id = 1, ProductId = product.Id},
             });

            var service = new OrdersService(ordersRepository, null, shoppingCartsServiceMock.Object, orderProductsRepository, productsRepository, null);

            await service.CompleteOrderAsync(user.UserName);

            var orderProducts = await dbContext.OrderProducts.ToListAsync();

            orderProducts.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public async Task CompleteOrderAsyncShouldThrowNullReferenceExceptionWhenOrderWithUserUsernameNotExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, null, null, null, null);

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.CompleteOrderAsync("username"));
            Assert.Equal(string.Format(ExceptionMessages.OrderNotFound, "username"), exception.Message);
        }

        [Fact]
        public async Task CompleteOrderAsyncShouldThrowNullReferenceExceptionWhenOrderStatusIncorect()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var user = new ApplicationUser { Id = "1", Email = "username", UserName = "username" };
            var order = new Order { Id = 1, OrderStatus = OrderStatus.Delivered, ApplicationUser = user };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, null, null, null, null);

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.CompleteOrderAsync("username"));
            Assert.Equal(string.Format(ExceptionMessages.OrderNotFound, "username"), exception.Message);
        }

        [Fact]
        public async Task CompleteOrderAsyncShouldThrowNullReferenceExceptionWhenShoppingCartProductsCountIsZero()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var user = new ApplicationUser { Id = "1", Email = "username", UserName = "username" };
            var order = new Order { Id = 1, OrderStatus = OrderStatus.Pending, ApplicationUser = user };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            var shoppingCartsServiceMock = new Mock<IShoppingCartsService>();
            shoppingCartsServiceMock.Setup(x => x.GetAllShoppingCartProducts("username"))
              .Returns(new List<ShoppingCartProductsViewModel>());

            using var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);

            var service = new OrdersService(ordersRepository, null, shoppingCartsServiceMock.Object, null, null, null);

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.CompleteOrderAsync("username"));
            Assert.Equal(string.Format(ExceptionMessages.InvalidShoppingCartProductsQuantity), exception.Message);
        }

        private Order SeedOrderDetails(int id, OrderStatus orderStatus, ShippingMethod shippingMethod, PaymentMethod paymentMethod, decimal totalPrice, ApplicationUser user, Address address)
        {
            return new Order
            {
                Id = id,
                OrderStatus = orderStatus,
                ShippingMethod = shippingMethod,
                PaymentMethod = paymentMethod,
                TotalPrice = totalPrice,
                ApplicationUser = user,
                Address = address,
            };
        }

        private void InitializeMapper() => AutoMapperConfig.
       RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
    }
}
