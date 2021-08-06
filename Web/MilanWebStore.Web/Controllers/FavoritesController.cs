namespace MilanWebStore.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.Favorites;

    [Authorize]
    public class FavoritesController : BaseController
    {
        private readonly IFavoriteProductsService favoritesService;
        private readonly UserManager<ApplicationUser> usermanger;

        public FavoritesController(
            IFavoriteProductsService favoritesService,
            UserManager<ApplicationUser> usermanger)
        {
            this.favoritesService = favoritesService;
            this.usermanger = usermanger;
        }

        public IActionResult Index()
        {
            var userFavoriteProducts = new AllFavoriteProductsViewModel()
            {
                Favorites = this.favoritesService.All<FavoriteProductViewModel>(this.User.Identity.Name),
            };

            return this.View(userFavoriteProducts);
        }

        public async Task<IActionResult> Add(int id)
        {
            var user = await this.usermanger.GetUserAsync(this.User);
            await this.favoritesService.AddAsync(user.Id, id);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.favoritesService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
