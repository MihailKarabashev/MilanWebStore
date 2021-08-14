namespace MilanWebStore.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.Addresses;

    public class AddressesController : BaseController
    {
        private readonly IAddressesService addressesService;
        private readonly UserManager<ApplicationUser> userManager;

        public AddressesController(
            IAddressesService addressesService,
            UserManager<ApplicationUser> userManager)
        {
            this.addressesService = addressesService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Add(AddressViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Create", "Orders");
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.addressesService.CreateAsync(model, user.Id);

            return this.RedirectToAction("Create", "Orders");
        }
    }
}
