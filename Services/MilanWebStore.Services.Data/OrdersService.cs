using Microsoft.EntityFrameworkCore;
using MilanWebStore.Common;
using MilanWebStore.Data.Common.Repositories;
using MilanWebStore.Data.Models;
using MilanWebStore.Data.Models.Enums;
using MilanWebStore.Services.Data.Contracts;
using MilanWebStore.Services.Mapping;
using MilanWebStore.Web.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilanWebStore.Services.Data
{
    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;
        private readonly IUsersService usersService;
        private readonly IShoppingCartsService shoppingCartsService;
        private readonly IRepository<OrderProduct> orderProductsRepository;
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly IRepository<Address> addressesRepository;

        public OrdersService(
            IDeletableEntityRepository<Order> ordersRepository,
            IUsersService usersService,
            IShoppingCartsService shoppingCartsService,
            IRepository<OrderProduct> orderProductsRepository,
            IDeletableEntityRepository<Product> productsRepository,
            IRepository<Address> addressesRepository
            )
        {
            this.ordersRepository = ordersRepository;
            this.usersService = usersService;
            this.shoppingCartsService = shoppingCartsService;
            this.orderProductsRepository = orderProductsRepository;
            this.productsRepository = productsRepository;
            this.addressesRepository = addressesRepository;
        }

        public async Task CompleteOrderAsync(string username)
        {
            var order = this.ordersRepository.All().FirstOrDefault(x => x.ApplicationUser.Email == username
            && x.OrderStatus == OrderStatus.Pending);

            if (order == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.OrderNotFound, username));
            }

            var shoppingCartProducts = this.shoppingCartsService.GetAllShoppingCartProducts(username);

            if (shoppingCartProducts.Count() == 0 || shoppingCartProducts == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.InvalidShoppingCartProductsQuantity));

            }

            var orderProductsList = new List<OrderProduct>();
            decimal orderTotalPrice = 0;

            foreach (var shoppingCartProduct in shoppingCartProducts)
            {
                var product = this.productsRepository.All().FirstOrDefault(x => x.Id == shoppingCartProduct.ProductId);


                var orderProduct = new OrderProduct()
                {
                    Order = order,
                    Product = product,
                    Price = shoppingCartProduct.Price * shoppingCartProduct.Quantity,
                    Quantity = shoppingCartProduct.Quantity,
                };

                orderTotalPrice += orderProduct.Price;
                orderProductsList.Add(orderProduct);

                await this.orderProductsRepository.AddAsync(orderProduct);
            }

            order.TotalPrice = orderTotalPrice;
            order.OrderStatus = OrderStatus.NotDelivered;
            order.PaymentStatus = PaymentStatus.UnPaid;


            await this.shoppingCartsService.ClearShoppingCartAsync(username);

            await this.orderProductsRepository.SaveChangesAsync();

        }

        public async Task SetOrderDetailsAsync(OrderInputModel input, string username)
        {
            var order = await this.CreateAsync(username);

            if (order == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.OrderNotFound, username));
            }

            var address = this.addressesRepository.All().FirstOrDefault(x => x.Id == input.Address.Id && x.ApplicationUser.UserName == username);

            order.Address = address;
            order.AddressId = address.Id;
            order.PaymentMethod = input.PaymentMethod;
            order.ShippingMethod = input.ShippingMethod;
            order.ApplicationUser.FirstName = input.FirstName;
            order.ApplicationUser.LastName = input.LastName;
            order.ApplicationUser.PhoneNumber = input.PhoneNumber;

            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();
        }

        public T GetProcessedOrder<T>(string username)
        {
            return this.ordersRepository.All().Where(x => x.ApplicationUser.UserName == username
             && x.OrderStatus == OrderStatus.Pending).To<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetAllOrderProductsById<T>(int orderId)
        {
            return this.orderProductsRepository.All().
                Where(x => x.OrderId == orderId)
                 .To<T>().ToList();
        }

        public Order FindOrderById(int orderId)
        {
            return this.ordersRepository.All().Include(x => x.Address).ThenInclude(x => x.ApplicationUser)
                .FirstOrDefault(x => x.Id == orderId);
        }

        public decimal GetAllOrderProductTotalPrice(int orderId)
        {
            return this.orderProductsRepository.All().Where(x => x.OrderId == orderId)
                   .Sum(x => x.Product.DiscountPrice != null ? (decimal)x.Product.DiscountPrice * x.Quantity : x.Product.Price * x.Quantity);
        }

        public T GetOrderById<T>(int id)
        {
            return this.ordersRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetUserOrders<T>(string username)
        {
            return this.ordersRepository.All().
                Where(x => x.ApplicationUser.UserName == username && x.OrderStatus != OrderStatus.Delivered)
                .OrderByDescending(x => x.Id)
                .To<T>().ToList();
        }

        public IEnumerable<T> GetUnProcessedOrders<T>()
        {
            return this.ordersRepository.All().Where(x => x.OrderStatus == OrderStatus.NotDelivered)
                 .OrderByDescending(x => x.CreatedOn).To<T>().ToList();
        }

        public IEnumerable<T> GetProcessedOrders<T>()
        {
            return this.ordersRepository.All().Where(x => x.OrderStatus == OrderStatus.Pending)
                .OrderByDescending(x => x.CreatedOn).To<T>().ToList();
        }

        public IEnumerable<T> GetDeliveredOrders<T>()
        {
            return this.ordersRepository.All().Where(x => x.OrderStatus == OrderStatus.Delivered)
                .OrderByDescending(x => x.CreatedOn).To<T>().ToList();
        }


        public async Task ProcessOrder(int id)
        {
            var order = this.ordersRepository.All().Where(x => x.Id == id && x.OrderStatus == OrderStatus.NotDelivered)
                .FirstOrDefault();

            if (order == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.OrderIdNotFound, id));
            }

            order.OrderStatus = OrderStatus.Pending;
            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();
        }

        public async Task DeliverOrder(int id)
        {
            var order = this.ordersRepository.All().Where(x => x.Id == id && x.OrderStatus == OrderStatus.Pending)
             .FirstOrDefault();

            if (order == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.OrderIdNotFound, id));
            }

            order.OrderStatus = OrderStatus.Delivered;
            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();
        }

        public async Task UpdatePaymentStatus(int orderId)
        {
            var order = this.ordersRepository.All().
                Where(x => x.Id == orderId && x.PaymentStatus == PaymentStatus.UnPaid)
                .FirstOrDefault();

            if (order == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.OrderIdNotFound, orderId));
            }

            order.PaymentStatus = PaymentStatus.Paid;

            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();
        }

        private async Task<Order> CreateAsync(string username)
        {
            var user = this.usersService.GetUser(username);

            var order = new Order()
            {
                ApplicationUser = user,
                OrderStatus = OrderStatus.Pending,
            };

            await this.ordersRepository.AddAsync(order);
            await this.ordersRepository.SaveChangesAsync();

            return order;
        }
    }
}
