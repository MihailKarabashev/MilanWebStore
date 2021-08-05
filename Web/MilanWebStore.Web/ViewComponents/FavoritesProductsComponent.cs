namespace MilanWebStore.Web.ViewComponents
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.Favorites;

    [Authorize]
    public class FavoritesProductsComponent : ViewComponent
    {
        private readonly IFavoriteProductsService favoriteProductsService;

        public FavoritesProductsComponent(IFavoriteProductsService favoriteProductsService)
        {
            this.favoriteProductsService = favoriteProductsService;
        }

        public IViewComponentResult Invoke()
        {
            var username = this.User.Identity.Name;
            var allFavoriteProducts = new AllFavoriteProductsViewModel()
            {
                Favorites = this.favoriteProductsService.All<FavoriteProductViewModel>(username),
            };
            return this.View(allFavoriteProducts);
        }
    }
}
