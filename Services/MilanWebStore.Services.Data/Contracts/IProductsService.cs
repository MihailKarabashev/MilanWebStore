namespace MilanWebStore.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MilanWebStore.Data.Models;
    using MilanWebStore.Web.ViewModels.Administration.Products;
    using MilanWebStore.Web.ViewModels.Products;

    public interface IProductsService
    {
        Task CreateAsync(ProductInputModel model, string imagePath);

        Task EditAsync(ProductEditViewModel model);

        T GetById<T>(int id);

        Task DeleteAsync(int id);

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 4);

        IEnumerable<T> GetAllInSale<T>();

        IEnumerable<T> GetTopProducts<T>();

        int GetCountByParentAndChildId(int parentId, int? childId);

        int GetProductsCount();

        IEnumerable<T> GetRelatedProducts<T>(int categoryId, int id);

        IEnumerable<T> FilterByCriteria<T>(AllProductsQueryModel model, int page, int itemsPerPage);

        ProductOfTheWeekViewModel DealOfTheWeek();

        Product FindById(int id);
    }
}
