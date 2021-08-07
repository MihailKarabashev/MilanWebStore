namespace MilanWebStore.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using MilanWebStore.Common;
    using MilanWebStore.Data.Common.Repositories;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Services.Mapping;
    using MilanWebStore.Web.ViewModels;
    using MilanWebStore.Web.ViewModels.Administration.Products;
    using MilanWebStore.Web.ViewModels.Products;

    public class ProductsService : IProductsService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };
        private readonly IDeletableEntityRepository<Product> productsRepository;

        public ProductsService(
            IDeletableEntityRepository<Product> productsRepository)
        {
            this.productsRepository = productsRepository;
        }

        public T GetById<T>(int id)
        {
            //var ss = this.productsRepository.All().Where(x => x.Id == id)
            //    .Select(x => new TestProduct
            //    {
            //        Name = x.Name,
            //        ChildCategoryId = x.ChildCategoryId,
            //        ParentCategoryId = x.ParentCategoryId,
            //        Price = x.Price,
            //        Id = x.Id,
            //        DiscountPrice = x.DiscountPrice,
            //    })
            //    .FirstOrDefault();

            return this.productsRepository.All().Where(x => x.Id == id)
                .To<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetAllInSale<T>()
        {
            return this.productsRepository.All()
                 .Where(x => x.Availiability && x.InDiscount)
                 .Take(4)
                 .OrderByDescending(x => x.DiscountPrice).To<T>().ToList();
        }

        public IEnumerable<T> GetTopProducts<T>()
        {
            return this.productsRepository.All().Where(x => x.Availiability && !x.InDiscount)
                .Take(4)
                .OrderByDescending(x => x.Price).To<T>().ToList();
        }

        public async Task EditAsync(ProductEditViewModel model)
        {
            var product = this.FindById(model.Id);

            if (product == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ProductNotFound, model.Id));
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.DiscountPrice = model.DiscountPrice;
            product.ParentCategoryId = model.ParentCategoryId;
            product.ChildCategoryId = model.ChildCategoryId;

            this.productsRepository.Update(product);
            await this.productsRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetRelatedProducts<T>(int categoryId, int id)
        {
            return this.productsRepository.All().Where(x => x.ParentCategoryId == categoryId && x.Id != id)
                .Take(4).OrderByDescending(x => x.Price)
                .To<T>().ToList();
        }

        public int GetCountByParentAndChildId(int parentId, int? childId)
        {
            if (childId == null)
            {
                return this.productsRepository.All().Count(x => x.ParentCategoryId == parentId);
            }

            return this.productsRepository
                .All()
                .Count(x => x.ParentCategoryId == parentId && x.ChildCategoryId == childId);
        }

        public async Task CreateAsync(ProductInputModel model, string imagePath)
        {
            var product = new Product()
            {
                Name = model.Name,
                ChildCategoryId = model.ChildCategoryId,
                ParentCategoryId = model.ParentCategoryId,
                Description = model.Description,
                Price = model.Price,
                DiscountPrice = model.DiscountPrice,
                Availiability = true,
            };

            if (product.DiscountPrice != null)
            {
                product.InDiscount = true;
            }

            product.ProductVariants.Add(new ProductVariant
            {
                ProductId = product.Id,
                SizeId = model.SizeId,
                IsSizeAvailable = true,
            });


            Directory.CreateDirectory($"{imagePath}/products/");
            foreach (var image in model.Images)
            {
                var extension = Path.GetExtension(image.FileName).TrimStart('.');

                if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
                {
                    throw new InvalidOperationException(string.Format(ExceptionMessages.InvalidImageExtension, extension));
                }

                var dbImage = new Image
                {
                    Extention = extension,
                };
                product.Images.Add(dbImage);

                var physicalPath = $"{imagePath}/products/{dbImage.Id}.{extension}";
                using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await image.CopyToAsync(fileStream);
            }

            await this.productsRepository.AddAsync(product);
            await this.productsRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 4)
        {
            return this.productsRepository.All()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .To<T>().ToList();
        }

        public async Task DeleteAsync(int id)
        {
            var product = this.FindById(id);

            if (product == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ProductNotFound, id));
            }

            product.ModifiedOn = DateTime.UtcNow;
            product.IsDeleted = true;

            this.productsRepository.Update(product);
            await this.productsRepository.SaveChangesAsync();
        }

        public ProductOfTheWeekViewModel DealOfTheWeek()
        {
            return this.productsRepository.All()
               .Where(x => x.DiscountPrice != null)
               .Select(x => new ProductOfTheWeekViewModel()
               {
                   Id = x.Id,
                   Name = x.Name,
                   Description = x.Description,
                   DiscountPrice = x.DiscountPrice,
                   ParentCategoryId = x.ParentCategoryId,
                   ChildCategoryId = x.ChildCategoryId,
                   Price = x.Price,
                   ImageUrl = x.Images.FirstOrDefault().RemoteUrl != null
                    ? x.Images.FirstOrDefault().RemoteUrl
                    : "/images/products/" + x.Images.FirstOrDefault().Id + "." + x.Images.FirstOrDefault().Extention,
                   Percentage = ((x.Price - (decimal)x.DiscountPrice) / x.Price) * 100,
               }).OrderByDescending(x => x.Percentage).FirstOrDefault();
        }

        public IEnumerable<T> FilterByCriteria<T>(AllProductsQueryModel model, int page, int itemsPerPage = 2)
        {
            var productsQuery = this.productsRepository.All().AsQueryable();

            if (model != null)
            {
                if (!string.IsNullOrWhiteSpace(model.SearchTerm))
                {
                    productsQuery = productsQuery.
                        Where(x => x.Name.ToLower().Contains(model.SearchTerm.ToLower()) ||
                              x.ChildCategory.Name.ToLower().Contains(model.SearchTerm.ToLower()) ||
                              x.ParentCategory.Name.ToLower().Contains(model.SearchTerm.ToLower()) ||
                              (x.Name + " " + x.Description).ToLower().Contains(model.SearchTerm.ToLower()));
                }

                if (model.SizeId.HasValue)
                {
                    productsQuery = productsQuery.Where(x => x.ProductVariants.Any(s => s.SizeId == model.SizeId && s.IsSizeAvailable));
                }

                if (model.ParentCategoryId.HasValue)
                {
                    productsQuery = productsQuery.Where(x => x.ParentCategoryId == model.ParentCategoryId);
                }

                if (model.ChildCategoryId.HasValue)
                {
                    productsQuery = productsQuery.Where(x => x.ChildCategoryId == model.ChildCategoryId);
                }


                productsQuery = model.FilterByPrice switch
                {
                    ProductPriceFilterViewModel.All =>
                    productsQuery.Where(x => (x.Price >= 1 && x.Price <= 3000) || (x.DiscountPrice >= 1 && x.DiscountPrice <= 3000)),

                    ProductPriceFilterViewModel.MinPrice =>
                    productsQuery.Where(x => (x.Price >= 7 && x.Price <= 50) || (x.DiscountPrice >= 7 && x.DiscountPrice <= 50)),

                    ProductPriceFilterViewModel.AveragePrice =>
                    productsQuery.Where(x => (x.Price >= 50 && x.Price <= 100) || (x.DiscountPrice >= 50 && x.DiscountPrice <= 100)),

                    ProductPriceFilterViewModel.HightPrice
                    => productsQuery.Where(x => (x.Price >= 100 && x.Price <= 200) || (x.DiscountPrice >= 100 && x.DiscountPrice <= 200)),

                    ProductPriceFilterViewModel.MaxPrice =>
                   productsQuery.Where(x => (x.Price >= 200 && x.Price <= 1000) || (x.DiscountPrice >= 200 && x.DiscountPrice <= 1000)),
                };
            }

            model.Paging = new PagingViewModel
            {
                ProductsCount = productsQuery.Count(),
            };

            var ss = productsQuery.To<T>().ToList();

            return productsQuery.OrderByDescending(x => x.DiscountPrice).ThenBy(p => p.Price)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .To<T>().ToList();
        }

        public int GetProductsCount()
        {
            return this.productsRepository.All().Count();
        }

        public Product FindById(int id)
        {
            return this.productsRepository.All().Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
