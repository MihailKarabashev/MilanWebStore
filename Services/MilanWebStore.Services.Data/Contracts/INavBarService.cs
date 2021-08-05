namespace MilanWebStore.Services.Data.Contracts
{
    using System.Collections.Generic;

    public interface INavBarService
    {
        IEnumerable<T> GetAllParentChildCategories<T>();
    }
}
