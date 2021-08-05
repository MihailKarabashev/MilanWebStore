namespace MilanWebStore.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INewsService
    {
        Task Scrape();

        T ById<T>(int id);

        IEnumerable<T> GetTop3LatestNews<T>();

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 6);

        int GetNewsCount();
    }
}
