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
    using MilanWebStore.Web.ViewModels.Administration.Sizes;

    public class SizesService : ISizesService
    {
        private readonly IDeletableEntityRepository<Size> sizesRepository;

        public SizesService(
            IDeletableEntityRepository<Size> sizesRepository)
        {
            this.sizesRepository = sizesRepository;
        }

        public async Task CreateAsync(SizeInputModel model)
        {
            var size = new Size()
            {
                Name = model.Name,
            };

            await this.sizesRepository.AddAsync(size);
            await this.sizesRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var size = this.sizesRepository.All().Where(x => x.Id == id).FirstOrDefault();

            if (size == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.SizeIdNotFoud, id));

            }

            size.ModifiedOn = DateTime.UtcNow;
            size.IsDeleted = true;

            this.sizesRepository.Update(size);
            await this.sizesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(EditSizeViewModel model)
        {
            var size = this.sizesRepository.All().Where(x => x.Id == model.Id).FirstOrDefault();

            if (size == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.SizeIdNotFoud, model.Id));
            }

            size.Name = model.Name;

            this.sizesRepository.Update(size);
            await this.sizesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.sizesRepository.All().To<T>().ToList();
        }

        public T GetById<T>(int id)
        {
            return this.sizesRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();
        }
    }
}
