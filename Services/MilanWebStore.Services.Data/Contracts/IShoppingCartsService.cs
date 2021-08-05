namespace MilanWebStore.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MilanWebStore.Web.ViewModels.ShoppingCarts;

    public interface IShoppingCartsService
    {
        IEnumerable<ShoppingCartProductsViewModel> GetAllShoppingCartProducts(string username);

        Task AddProductToShoppingCartAsync(int productId, string username, int quantity);

        Task EditShoppingCartProductsAsync(int shoppingCartProductId, string username, int quantity);

        Task DeleteShoppingCartProductAsync(int shoppingCartProductId, string username);

        bool AnyProducts(string username);

        Task ClearShoppingCartAsync(string username);
    }
}
