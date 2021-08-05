namespace MilanWebStore.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MilanWebStore.Data.Models;
    using MilanWebStore.Web.ViewModels.Orders;

    public interface IOrdersService
    {
        Task SetOrderDetailsAsync(OrderInputModel input, string username);

        Order FindOrderById(int orderId);

        Task CompleteOrderAsync(string username);

        T GetProcessedOrder<T>(string username);

        IEnumerable<T> GetAllOrderProductsById<T>(int orderId);

        decimal GetAllOrderProductTotalPrice(int orderId);

        Task UpdatePaymentStatus(int orderId);

        T GetOrderById<T>(int id);

        IEnumerable<T> GetUserOrders<T>(string username);

        IEnumerable<T> GetProcessedOrders<T>();

        IEnumerable<T> GetUnProcessedOrders<T>();

        IEnumerable<T> GetDeliveredOrders<T>();

        Task ProcessOrder(int id);

        Task DeliverOrder(int id);
    }
}
