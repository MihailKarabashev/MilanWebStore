namespace MilanWebStore.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFavoriteProductsService
    {
        IEnumerable<T> All<T>(string username);

        Task AddAsync(string userId, int productId);

        Task DeleteAsync(int favoriteProductId);
    }
}
