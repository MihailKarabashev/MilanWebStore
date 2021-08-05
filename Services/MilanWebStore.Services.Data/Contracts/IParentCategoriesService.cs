namespace MilanWebStore.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MilanWebStore.Web.ViewModels.Administration.ParentCategory;

    public interface IParentCategoriesService
    {
        IEnumerable<T> GetAll<T>();

        Task CreateAsync(ParentCategoryInputModel model);

        Task EditAsync(ParentCategoryViewModel model);

        T GetById<T>(int id);

        Task DeleteAsync(int id);
    }
}
