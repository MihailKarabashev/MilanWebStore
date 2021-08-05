namespace MilanWebStore.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MilanWebStore.Web.ViewModels.Administration.Sizes;

    public interface ISizesService
    {
        IEnumerable<T> GetAll<T>();

        Task CreateAsync(SizeInputModel model);

        Task EditAsync(EditSizeViewModel model);

        Task DeleteAsync(int id);

        T GetById<T>(int id);
    }
}
