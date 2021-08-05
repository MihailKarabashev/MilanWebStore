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

    public class ProductVariantsService : IProductVariantsService
    {
        private readonly IDeletableEntityRepository<ProductVariant> productVariantsRepository;
        private readonly ProductsService productsService;
        private readonly SizesService sizesService;

        public ProductVariantsService(
            IDeletableEntityRepository<ProductVariant> productVariantsRepository
            //ProductsService productsService,
            /*SizesService sizesService*/)
        {
            this.productVariantsRepository = productVariantsRepository;
            //this.productsService = productsService;
            //this.sizesService = sizesService;
        }

        public async Task AddAsync(int sizeId, int productId)
        {
            //var product = this.productsService.FindById(productId);

            //if (product == null)
            //{
            //    throw new NullReferenceException(string.Format(ExceptionMessages.ProductNotFound, productId));
            //}

            //var size = this.sizesService.GetById<Size>(sizeId);

            //if (size == null)
            //{
            //    throw new NullReferenceException(string.Format(ExceptionMessages.SizeIdNotFoud, sizeId));
            //}

            var productVariant = new ProductVariant()
            {
                ProductId = productId,
                SizeId = sizeId,
                IsSizeAvailable = true,
            };

            await this.productVariantsRepository.AddAsync(productVariant);
            await this.productVariantsRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllVariants<T>(int id)
        {
            return this.productVariantsRepository.All().Where(x => x.IsSizeAvailable && x.ProductId == id).To<T>().ToList();
        }

        public async Task RemoveAsync(int id)
        {
            var singleProductVariant = this.productVariantsRepository.All().Where(x => x.Id == id).FirstOrDefault();

            if (singleProductVariant == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ProductVariantIdNotFound, id));

            }

            singleProductVariant.ModifiedOn = DateTime.UtcNow;
            singleProductVariant.IsDeleted = true;

            this.productVariantsRepository.Update(singleProductVariant);
            await this.productVariantsRepository.SaveChangesAsync();
        }

    }
}
