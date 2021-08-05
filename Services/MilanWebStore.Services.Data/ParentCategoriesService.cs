namespace MilanWebStore.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MilanWebStore.Common;
    using MilanWebStore.Data.Common.Repositories;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Services.Mapping;
    using MilanWebStore.Web.ViewModels.Administration.ParentCategory;

    public class ParentCategoriesService : IParentCategoriesService
    {
        private readonly IDeletableEntityRepository<ParentCategory> parentCategoriesRepository;

        public ParentCategoriesService(
            IDeletableEntityRepository<ParentCategory> parentCategoriesRepository)
        {
            this.parentCategoriesRepository = parentCategoriesRepository;
        }

        public async Task CreateAsync(ParentCategoryInputModel model)
        {
            var parentCategory = new ParentCategory()
            {
                Name = model.Name,
            };

            await this.parentCategoriesRepository.AddAsync(parentCategory);
            await this.parentCategoriesRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var parentCategory = this.FindById(id);

            if (parentCategory == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ParentCategoryNotFound, id));
            }

            parentCategory.ModifiedOn = DateTime.UtcNow;
            parentCategory.IsDeleted = true;

            this.parentCategoriesRepository.Update(parentCategory);
            await this.parentCategoriesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(ParentCategoryViewModel model)
        {
            var parentCategory = this.FindById(model.Id);

            if (parentCategory == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ParentCategoryNotFound, model.Id));
            }

            parentCategory.Name = model.Name;

            this.parentCategoriesRepository.Update(parentCategory);
            await this.parentCategoriesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.parentCategoriesRepository.All().To<T>().ToList();
        }

        public T GetById<T>(int id)
        {
            return this.parentCategoriesRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();
        }

        private ParentCategory FindById(int id)
        {
            return this.parentCategoriesRepository.All().Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
