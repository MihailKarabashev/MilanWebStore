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
    using MilanWebStore.Services.Mapping;

    public class FavoriteProductsService : IFavoriteProductsService
    {
        private readonly IRepository<FavoriteProduct> favoritesRepository;
        private readonly IProductsService productsService;
        private readonly IUsersService usersService;

        public FavoriteProductsService(
            IRepository<FavoriteProduct> favoritesRepository,
            IProductsService productsService,
            IUsersService usersService)
        {
            this.favoritesRepository = favoritesRepository;
            this.productsService = productsService;
            this.usersService = usersService;
        }

        public async Task AddAsync(string userId, int productId)
        {
            var product = this.productsService.FindById(productId);

            if (product == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ProductNotFound, productId));
            }

            var user = this.usersService.GetUserById(userId);

            if (user == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.UserIdNotFound, userId));
            }

            var favoriteProduct = new FavoriteProduct()
            {
                ApplicationUserId = userId,
                ProductId = productId,
            };

            await this.favoritesRepository.AddAsync(favoriteProduct);
            await this.favoritesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> All<T>(string username)
        {
            var favoriteProducts = this.favoritesRepository.All().Where(x => x.ApplicationUser.UserName == username).To<T>().ToList();

            if (favoriteProducts == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.UserNameNotFound, username));
            }

            return favoriteProducts;
        }

        public async Task DeleteAsync(int favoriteProductId)
        {
            var favoriteProduct = await this.favoritesRepository.All().FirstOrDefaultAsync(x => x.Id == favoriteProductId);

            if (favoriteProduct == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.FavoriteProductIdNotFound, favoriteProductId));
            }

            this.favoritesRepository.Delete(favoriteProduct);
            await this.favoritesRepository.SaveChangesAsync();
        }
    }
}
