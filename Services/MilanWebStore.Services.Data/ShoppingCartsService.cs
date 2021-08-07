namespace MilanWebStore.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Common;
    using MilanWebStore.Data.Common.Repositories;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.ShoppingCarts;

    public class ShoppingCartsService : IShoppingCartsService
    {
        private readonly IDeletableEntityRepository<ShoppingCartProduct> shoppingCartProductsRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly IRepository<ProductVariant> productVariantsRepository;

        public ShoppingCartsService(
            IDeletableEntityRepository<ShoppingCartProduct> shoppingCartProductsRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<Product> productsRepository,
            IRepository<ProductVariant> productVariantsRepository)
        {
            this.shoppingCartProductsRepository = shoppingCartProductsRepository;
            this.usersRepository = usersRepository;
            this.productsRepository = productsRepository;
            this.productVariantsRepository = productVariantsRepository;
        }

        public async Task AddProductToShoppingCartAsync(int productId, string username, int quantity)
        {
            var product = await this.productsRepository.All().FirstOrDefaultAsync(v => v.Id == productId);

            if (product == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ProductNotFound, productId));
            }

            var user = await this.usersRepository.All().FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.UserNameNotFound, username));
            }

            if (quantity <= 0)
            {
                throw new InvalidOperationException(ExceptionMessages.InvalidQuantity);
            }

            var shoppingCartProduct = new ShoppingCartProduct()
            {
                Product = product,
                ShoppingCartId = user.ShoppingCartId,
                Quantity = quantity,
            };


            await this.shoppingCartProductsRepository.AddAsync(shoppingCartProduct);
            await this.shoppingCartProductsRepository.SaveChangesAsync();
        }

        public bool AnyProducts(string username)
        {
            return this.shoppingCartProductsRepository.All().Any(x => x.ShoppingCart.ApplicationUser.UserName == username);
        }

        public async Task ClearShoppingCartAsync(string username)
        {
            var user = await this.usersRepository.All().FirstOrDefaultAsync(x => x.UserName == username);
            if (user == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.UserNameNotFound, username));
            }

            var shoppingCartProducts = this.shoppingCartProductsRepository
                .All()
                .Where(u => u.ShoppingCart.ApplicationUser == user)
                .ToList();

            foreach (var shoppingCartProduct in shoppingCartProducts)
            {
                shoppingCartProduct.IsDeleted = true;
                this.shoppingCartProductsRepository.Update(shoppingCartProduct);
            }

            await this.shoppingCartProductsRepository.SaveChangesAsync();
        }

        public async Task DeleteShoppingCartProductAsync(int shoppingCartProductId, string username)
        {
            var shoppingCartProduct = await this.shoppingCartProductsRepository.All()
                 .FirstOrDefaultAsync(x => x.Id == shoppingCartProductId);

            if (shoppingCartProduct == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ShoppingCartProductIdNotFound, shoppingCartProductId));
            }

            var user = await this.usersRepository.All().FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.UserNameNotFound, username));
            }

            this.shoppingCartProductsRepository.Delete(shoppingCartProduct);
            await this.shoppingCartProductsRepository.SaveChangesAsync();
        }

        public async Task EditShoppingCartProductsAsync(int shoppingCartProductId, string username, int quantity)
        {
            var shoppingCartProduct = await this.shoppingCartProductsRepository.All()
                 .FirstOrDefaultAsync(x => x.Id == shoppingCartProductId);

            if (shoppingCartProduct == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ShoppingCartProductIdNotFound, shoppingCartProductId));
            }

            var user = await this.usersRepository.All().FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.UserNameNotFound, username));
            }

            if (quantity <= 0)
            {
                throw new InvalidOperationException(ExceptionMessages.InvalidQuantity);
            }

            shoppingCartProduct.Quantity = quantity;
            this.shoppingCartProductsRepository.Update(shoppingCartProduct);
            await this.shoppingCartProductsRepository.SaveChangesAsync();
        }

        public IEnumerable<ShoppingCartProductsViewModel> GetAllShoppingCartProducts(string username)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.UserNameNotFound, username));
            }

            var shoppingCartProducts = this.shoppingCartProductsRepository.All()
                .Where(x => x.ShoppingCart.ApplicationUser.UserName == user.UserName)
                .Include(x => x.Product)
                .ThenInclude(x => x.Images)
                .Include(x => x.ShoppingCart)
                .Include(x => x.Product.ProductVariants)
                .ToList();

            var shoppingCartProductsViewModel = shoppingCartProducts.Select(x => new ShoppingCartProductsViewModel()
            {
                Id = x.Id,
                ProductName = x.Product.Name,
                ProductImageUrl = x.Product.Images.FirstOrDefault().RemoteUrl != null
                   ? x.Product.Images.FirstOrDefault().RemoteUrl
                   : "/images/products/" + x.Product.Images.FirstOrDefault().Id + "." + x.Product.Images.FirstOrDefault().Extention,
                Price = x.Product.DiscountPrice == null ? x.Product.Price : (decimal)x.Product.DiscountPrice,
                Quantity = x.Quantity,
                ProductId = x.ProductId,
                ShoppingCartApplicationUserId = x.ShoppingCart.ApplicationUser.Id,
            }).ToList();

            return shoppingCartProductsViewModel;
        }
    }
}
