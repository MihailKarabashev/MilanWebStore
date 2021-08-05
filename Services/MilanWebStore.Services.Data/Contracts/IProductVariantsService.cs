namespace MilanWebStore.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductVariantsService
    {
        IEnumerable<T> GetAllVariants<T>(int id);

        Task RemoveAsync(int id);

        Task AddAsync(int sizeId, int productId);
    }
}
