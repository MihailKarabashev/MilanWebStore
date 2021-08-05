namespace MilanWebStore.Services.Data
{
    using System.Collections.Generic;

    using MilanWebStore.Data.Common.Repositories;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Services.Mapping;

    public class NavBarService : INavBarService
    {
        private readonly IDeletableEntityRepository<ParentCategory> parentCategoryRepository;

        public NavBarService(IDeletableEntityRepository<ParentCategory> parentCategoryRepository)
        {
            this.parentCategoryRepository = parentCategoryRepository;
        }

        public IEnumerable<T> GetAllParentChildCategories<T>()
        {
            return this.parentCategoryRepository.All().To<T>().ToList();
        }
    }
}
