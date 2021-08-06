namespace MilanWebStore.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Common;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.Helpers;
    using MilanWebStore.Web.ViewModels.ShoppingCarts;

    public class ShoppingCartsController : BaseController
    {
        private readonly IShoppingCartsService shoppingCartsService;
        private readonly IProductsService productsService;

        public ShoppingCartsController(
            IShoppingCartsService shoppingCartsService,
            IProductsService productsService)
        {
            this.shoppingCartsService = shoppingCartsService;
            this.productsService = productsService;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var username = this.User.Identity.Name;
                var shoppingCartProducts = this.shoppingCartsService.GetAllShoppingCartProducts(username);

                if (shoppingCartProducts.Count() == 0 || shoppingCartProducts == null)
                {
                    return this.RedirectToAction("Index", "Home");
                }

                return this.View(shoppingCartProducts);
            }

            var shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(this.HttpContext.Session, GlobalConstants.ShoppingCartKey);

            if (shoppingCartSession.Count() == 0 || shoppingCartSession == null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View(shoppingCartSession);
        }

        public async Task<IActionResult> Add(int id, int quantity = 1)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var username = this.User.Identity.Name;
                await this.shoppingCartsService.AddProductToShoppingCartAsync(id, username, quantity);
            }
            else
            {
                this.AddToShoppingCartWithSession(id, quantity);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Edit(int shoppingCartId, int quantity)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var username = this.User.Identity.Name;
                await this.shoppingCartsService.EditShoppingCartProductsAsync(shoppingCartId, username, quantity);
            }
            else
            {
                var shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(this.HttpContext.Session, GlobalConstants.ShoppingCartKey);

                this.EditShoppingCartWithSession(shoppingCartSession, shoppingCartId, quantity);

                if (shoppingCartSession == null)
                {
                    return this.RedirectToAction(nameof(ProductsController.All));
                }
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(int shoppingCartId)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var username = this.User.Identity.Name;
                await this.shoppingCartsService.DeleteShoppingCartProductAsync(shoppingCartId, username);
            }
            else
            {
                var shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(this.HttpContext.Session, GlobalConstants.ShoppingCartKey);

                if (shoppingCartSession == null)
                {
                    return this.RedirectToAction(nameof(this.Index));
                }

                this.DeleteShoppingCartProductWithSession(shoppingCartSession, shoppingCartId);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        private void DeleteShoppingCartProductWithSession(List<ShoppingCartProductsViewModel> shoppingCartSession, int shoppingCartProductId)
        {
            if (shoppingCartSession.Any(x => x.Id == shoppingCartProductId))
            {
                var cart = shoppingCartSession.First(x => x.Id == shoppingCartProductId);

                shoppingCartSession.Remove(cart);

                SessionHelper.SetObjectAsJson(this.HttpContext.Session, GlobalConstants.ShoppingCartKey, shoppingCartSession);
            }
        }

        private void AddToShoppingCartWithSession(int id, int quantity)
        {
            var shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(this.HttpContext.Session, GlobalConstants.ShoppingCartKey);

            if (shoppingCartSession == null)
            {
                shoppingCartSession = new List<ShoppingCartProductsViewModel>();
            }

            if (!shoppingCartSession.Any(x => x.ProductId == id) && quantity > 0)
            {
                var product = this.productsService.GetById<ShoppingCartProductsViewModel>(id);
                product.Quantity = quantity;

                shoppingCartSession.Add(product);
            }

            SessionHelper.SetObjectAsJson(this.HttpContext.Session, GlobalConstants.ShoppingCartKey, shoppingCartSession);
        }

        private void EditShoppingCartWithSession(List<ShoppingCartProductsViewModel> shoppingCartSession, int shoppingCartId, int quantity)
        {
            if (shoppingCartSession != null)
            {
                if (shoppingCartSession.Any(x => x.Id == shoppingCartId) && quantity > 0)
                {
                    var cart = shoppingCartSession.Find(x => x.Id == shoppingCartId);

                    cart.Quantity = quantity;

                    SessionHelper.SetObjectAsJson(this.HttpContext.Session, GlobalConstants.ShoppingCartKey, shoppingCartSession);
                }
            }
        }
    }
}
