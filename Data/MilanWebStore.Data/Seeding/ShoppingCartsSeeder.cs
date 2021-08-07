namespace MilanWebStore.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Data.Models;

    public class ShoppingCartsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.ShoppingCarts.AnyAsync())
            {
                return;
            }

            var shoppingCart = new ShoppingCart()
            {
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
            };

            await dbContext.ShoppingCarts.AddAsync(shoppingCart);
        }
    }
}
