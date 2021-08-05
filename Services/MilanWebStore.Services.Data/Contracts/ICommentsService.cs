namespace MilanWebStore.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICommentsService
    {
        Task CreateAsync(int productId, string userId, string content);

        Task RemoveAsync(int id);

        IEnumerable<T> GetAll<T>(int productId);
    }
}
