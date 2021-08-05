
namespace MilanWebStore.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.Orders;

    public class OrderProductsComponent : ViewComponent
    {
        private readonly IOrdersService ordersService;

        public OrderProductsComponent(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public IViewComponentResult Invoke(int orderId)
        {
            var viewModel = new AllOrderProductsViewModel()
            {
                Products = this.ordersService.GetAllOrderProductsById<OrderProductViewModel>(orderId),
            };

            return this.View(viewModel);
        }
    }
}
