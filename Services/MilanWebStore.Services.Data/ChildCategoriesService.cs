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
    using MilanWebStore.Web.ViewModels.Administration.ChildCategory;

    public class ChildCategoriesService : IChildCategoriesService
    {
        private readonly IDeletableEntityRepository<ChildCategory> childCategoriesRepository;

        public ChildCategoriesService(IDeletableEntityRepository<ChildCategory> childCategoriesRepository)
        {
            this.childCategoriesRepository = childCategoriesRepository;
        }

        public async Task CreateAsync(ChildCategoryInputModel model)
        {
            var isChildExist = this.childCategoriesRepository.All().Any(x => x.Name == model.Name);

            if (isChildExist)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.ChildCategoryAlreadyExist, model.Name));
            }

            var childCategory = new ChildCategory()
            {
                Name = model.Name,
            };

            childCategory.ParentChildCategory.Add(new ParentChildCategory
            {
                ParentCateogryId = model.ParentCategoryId,
            });

            await this.childCategoriesRepository.AddAsync(childCategory);
            await this.childCategoriesRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var childCategory = this.childCategoriesRepository.All().Where(x => x.Id == id).FirstOrDefault();

            if (childCategory == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ChildCategoryNotFound, id));
            }

            childCategory.ModifiedOn = DateTime.UtcNow;
            childCategory.IsDeleted = true;

            this.childCategoriesRepository.Update(childCategory);
            await this.childCategoriesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(ChildCategoryViewModel model)
        {
            var childCategory = this.childCategoriesRepository.All().Where(x => x.Id == model.Id).FirstOrDefault();

            if (childCategory == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ChildCategoryNotFound, model.Id));
            }

            childCategory.Name = model.Name;

            this.childCategoriesRepository.Update(childCategory);
            await this.childCategoriesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.childCategoriesRepository.All().To<T>().ToList();
        }

        public T GetById<T>(int id)
        {
            return this.childCategoriesRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetAllKits<T>()
        {
            return this.childCategoriesRepository.All().Where(x => x.Name.Contains("Kit")).To<T>().ToList();
        }
    }
}
