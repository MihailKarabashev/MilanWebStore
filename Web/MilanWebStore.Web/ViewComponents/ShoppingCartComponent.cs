namespace MilanWebStore.Web.ViewComponents
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Common;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.Helpers;
    using MilanWebStore.Web.ViewModels.ShoppingCarts;

    public class ShoppingCartComponent : ViewComponent
    {
        private readonly IShoppingCartsService shoppingCartsService;

        public ShoppingCartComponent(IShoppingCartsService shoppingCartsService)
        {
            this.shoppingCartsService = shoppingCartsService;
        }

        public IViewComponentResult Invoke()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var username = this.User.Identity.Name;
                var shoppingCartProducts = this.shoppingCartsService.GetAllShoppingCartProducts(username);

                return this.View(shoppingCartProducts);
            }

            var shoppingCartSession = SessionHelper.
                GetObjectFromJson<List<ShoppingCartProductsViewModel>>(this.HttpContext.Session, GlobalConstants.ShoppingCartKey);

            if (shoppingCartSession == null)
            {
                shoppingCartSession = new List<ShoppingCartProductsViewModel>();
            }

            return this.View(shoppingCartSession);
        }
    }
}
