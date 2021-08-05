namespace MilanWebStore.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MilanWebStore.Web.ViewModels.Administration.ChildCategory;

    public interface IChildCategoriesService
    {
        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetAllKits<T>();

        Task CreateAsync(ChildCategoryInputModel model);

        Task EditAsync(ChildCategoryViewModel model);

        Task DeleteAsync(int id);

        T GetById<T>(int id);
    }
}
