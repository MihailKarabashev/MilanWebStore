namespace MilanWebStore.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Common;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.Addresses;
    using MilanWebStore.Web.ViewModels.Orders;

    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IShoppingCartsService shoppingCartService;
        private readonly IUsersService usersService;
        private readonly IAddressesService addressesService;
        private readonly IOrdersService ordersService;

        public OrdersController(
            IShoppingCartsService shoppingCartService,
            IUsersService usersService,
            IAddressesService addressesService,
            IOrdersService ordersService)
        {
            this.shoppingCartService = shoppingCartService;
            this.usersService = usersService;
            this.addressesService = addressesService;
            this.ordersService = ordersService;
        }

        public IActionResult Create()
        {
            var username = this.User.Identity.Name;

            if (!this.shoppingCartService.AnyProducts(username))
            {
                this.TempData["error"] = ExceptionMessages.TempDataOrderMessageForEmptyBasket;
                return this.RedirectToAction(nameof(HomeController.Index));
            }

            var address = this.addressesService.GetUserAddress<AddressViewModel>(username);
            var user = this.usersService.GetUser(username);

            var viewModel = new OrderInputModel()
            {
                Address = address,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderInputModel input)
        {
            var username = this.User.Identity.Name;

            if (!this.shoppingCartService.AnyProducts(username))
            {
                this.TempData["error"] = ExceptionMessages.TempDataOrderMessageForEmptyBasket;
                return this.RedirectToAction(nameof(HomeController.Index));
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.ordersService.SetOrderDetailsAsync(input, username);

            return this.RedirectToAction(nameof(this.Complete));
        }

        public async Task<IActionResult> Complete()
        {
            var username = this.User.Identity.Name;

            if (!this.shoppingCartService.AnyProducts(username))
            {
                this.TempData["error"] = ExceptionMessages.TempDataOrderMessageForEmptyBasket;
                return this.RedirectToAction(nameof(HomeController.Index));
            }

            var order = this.ordersService.GetProcessedOrder<CompleteOrderViewModel>(username);

            await this.ordersService.CompleteOrderAsync(username);

            return this.View(order);
        }

        public IActionResult MyOrders()
        {
            var username = this.User.Identity.Name;

            var userOrders = new UserOrdersViewModel()
            {
                Orders = this.ordersService.GetUserOrders<MyOrderViewModel>(username),
            };

            return this.View(userOrders);
        }
    }
}
