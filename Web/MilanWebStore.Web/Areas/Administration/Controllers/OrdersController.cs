namespace MilanWebStore.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.Administration.Orders;
    using MilanWebStore.Web.ViewModels.Orders;

    public class OrdersController : AdministrationController
    {
        private readonly IOrdersService ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public IActionResult Index()
        {
            var ordersViewModel = new IndexOrdersViewModel()
            {
                ProcessedOrders = this.ordersService.GetProcessedOrders<MyOrderViewModel>(),
                UnProcessedOrders = this.ordersService.GetUnProcessedOrders<MyOrderViewModel>(),
            };

            return this.View(ordersViewModel);
        }

        public async Task<IActionResult> Process(int id)
        {
            await this.ordersService.ProcessOrder(id);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Deliver(int id)
        {
            await this.ordersService.DeliverOrder(id);

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Delivered()
        {
            var viewModel = new DeliveredOrdersViewModel()
            {
                Orders = this.ordersService.GetDeliveredOrders<MyOrderViewModel>(),
            };

            return this.View(viewModel);
        }
    }
}
