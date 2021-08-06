namespace MilanWebStore.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Common;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.Payments;

    [Authorize]
    public class PaymentsController : BaseController
    {
        private readonly IPaymentsService paymentsService;
        private readonly IOrdersService ordersService;

        public PaymentsController(
            IPaymentsService paymentsService,
            IOrdersService ordersService)
        {
            this.paymentsService = paymentsService;
            this.ordersService = ordersService;
        }

        public IActionResult Pay(int orderId)
        {
            var payModel = new PayInputModel()
            {
                OrderId = orderId,
                Amount = this.ordersService.GetAllOrderProductTotalPrice(orderId),
            };

            return this.View(payModel);
        }

        [HttpPost]
        public async Task<IActionResult> Pay(PayInputModel payModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(payModel);
            }

            var result = await this.paymentsService.ChargeAsync(payModel);

            if (result == "Success")
            {
                return this.RedirectToAction(nameof(this.SuccessfulPayment), new { id = payModel.OrderId });
            }
            else
            {
                return this.RedirectToAction(nameof(this.UnsuccessfulPayment), new { id = payModel.OrderId });
            }
        }

        public async Task<IActionResult> SuccessfulPayment(int id)
        {
            await this.ordersService.UpdatePaymentStatus(id);

            return this.View();
        }

        public IActionResult UnsuccessfulPayment()
        {
            this.TempData["info"] = ExceptionMessages.UnsuccessfullPayment;

            return this.RedirectToAction(nameof(HomeController.Index));
        }
    }
}
