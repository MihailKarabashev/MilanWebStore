namespace MilanWebStore.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Common;
    using MilanWebStore.Data.Models;

    public class ProductsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Products.AnyAsync())
            {
                return;
            }

            var tshirtId = await dbContext
                .ChildCategories.Where(x => x.Name == "T-Shirts")
                .Select(x => x.Id)
                .FirstOrDefaultAsync();


            var homeKitId = await dbContext
              .ChildCategories.Where(x => x.Name == "Home Kit")
              .Select(x => x.Id)
              .FirstOrDefaultAsync();


            var awayKitId = await dbContext
              .ChildCategories.Where(x => x.Name == "Away Kit")
              .Select(x => x.Id)
              .FirstOrDefaultAsync();

            var thirdKitId = await dbContext
             .ChildCategories.Where(x => x.Name == "Third Kit")
             .Select(x => x.Id)
             .FirstOrDefaultAsync();

            var socksId = await dbContext
            .ChildCategories.Where(x => x.Name == "Socks")
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

            var manId = await dbContext
             .ParentCategories.Where(x => x.Name == "Men")
             .Select(x => x.Id)
             .FirstOrDefaultAsync();

            var products = new List<Product>()
            {
#pragma warning disable SA1413 // Use trailing comma in multi-line initializers
                new Product
                {
                   Name = "Ac Milan Black Casuals Presentation T-shirt 2021/22",
                   Description = "80% cotton, 20% polyester. Short sleeved presentation t-shirt with crew neckline. AC Milan crest and Puma logo.",
                   Price = 36,
                   InDiscount = false,
                   Availiability = true,
                   ChildCategoryId = tshirtId,
                   ParentCategoryId = manId,
                   CreatedOn = DateTime.UtcNow,
                   IsDeleted = false,
                   ProductVariants = new List<ProductVariant>
                   {
                       new ProductVariant { SizeId = 1, IsSizeAvailable = true},
                       new ProductVariant { SizeId = 2, IsSizeAvailable = true},
                       new ProductVariant { SizeId = 3, IsSizeAvailable = true},
                   },
                   Images = new List<Image>
                   {
                       new Image { RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/M/I/MI21B01.jpg"},
                       new Image { RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/M/I/MI21B01_1.jpg"}
                   },
                },
                new Product
                {
                    Name = "Ac Milan Afterglow Casuals Presentation T-shirt 2021/22",
                    Description = "80% cotton, 20% polyester. Short sleeved presentation t-shirt with crew neckline. AC Milan crest and Puma logo.",
                    Price = 36,
                    DiscountPrice = 22,
                    InDiscount = true,
                    Availiability = true,
                    ChildCategoryId = tshirtId,
                    ParentCategoryId = manId,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false,
                    ProductVariants = new List<ProductVariant>
                    {
                       new ProductVariant { SizeId = 3, IsSizeAvailable = true},
                       new ProductVariant { SizeId = 2, IsSizeAvailable = true},
                       new ProductVariant { SizeId = 4, IsSizeAvailable = true },
                   },
                    Images = new List<Image>
                   {
                       new Image { RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/M/I/MI21B02.jpg"},
                       new Image { RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/M/I/MI21B02_1.jpg"}
                   },
                },
                new Product
                {
                    Name = "Ac Milan Ss Home Jersey 2021/22",
                    Description = "80% cotton, 20% polyester. Short sleeved presentation t-shirt with crew neckline. AC Milan crest and Puma logo.",
                    Price = 90,
                    InDiscount = false,
                    Availiability = true,
                    ChildCategoryId = homeKitId,
                    ParentCategoryId = manId,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false,
                     ProductVariants = new List<ProductVariant>
                    {
                       new ProductVariant { SizeId = 1, IsSizeAvailable = true},
                       new ProductVariant { SizeId = 2, IsSizeAvailable = true},
                       new ProductVariant { SizeId = 4 , IsSizeAvailable = true},
                    },
                    Images = new List<Image>
                    {
                       new Image { RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/M/I/MI21A01.jpg"},
                       new Image { RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/M/I/MI21A01_1.jpg"},
                       new Image { RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/M/I/MI21A01_2.jpg"},
                       new Image { RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/m/i/mi21a01_3.jpg"}
                    },
                },
                  new Product
                {
                    Name = "Ac Milan Ss Away Jersey 2021/22",
                    Description = "100% polyester Recycled - double face jacquard - 165.00 g/m² - print - Chemical - Absorbency&/or Wicking - DRYCELL (FUN/001)",
                    Price = 90,
                    InDiscount = false,
                    Availiability = true,
                    ChildCategoryId = awayKitId,
                    ParentCategoryId = manId,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false,
                     ProductVariants = new List<ProductVariant>
                    {
                       new ProductVariant { SizeId = 1, IsSizeAvailable = true},
                       new ProductVariant { SizeId = 2, IsSizeAvailable = true},
                       new ProductVariant { SizeId = 3, IsSizeAvailable = true},
                       new ProductVariant { SizeId = 4, IsSizeAvailable = true},
                    },
                    Images = new List<Image>
                    {
                       new Image { RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/M/I/MI21A02.jpg"},
                       new Image { RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/M/I/MI21A02_1.jpg"},
                       new Image {RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/M/I/MI21A02_2.jpg"},
                       new Image {RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/M/I/MI21A02_3.jpg"},
                    },
                },
                 new Product
                {
                    Name = "Ac Milan Home Goalkeeper Jersey 2021/22",
                    Description = "100% polyester. Short sleeved gameday jersey with sponsor. AC Milan and Puma logo",
                    Price = 90,
                    InDiscount = false,
                    Availiability = true,
                    ChildCategoryId = thirdKitId,
                    ParentCategoryId = manId,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false,
                     ProductVariants = new List<ProductVariant>
                    {
                       new ProductVariant { SizeId = 1, IsSizeAvailable = true},
                       new ProductVariant { SizeId = 2, IsSizeAvailable = true},
                       new ProductVariant { SizeId = 3, IsSizeAvailable = true},
                       new ProductVariant { SizeId = 4, IsSizeAvailable = true},
                    },
                    Images = new List<Image>
                    {
                       new Image { RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/M/I/MI21A04.jpg"},
                       new Image { RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/M/I/MI21A04_1.jpg"},
                       new Image {RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/M/I/MI21A04_2.jpg"},
                       new Image {RemoteUrl = "https://dn1mx6r42x958.cloudfront.net/media/catalog/product/cache/c74aeb9d0072ba9e76949435707f3a08/M/I/MI21A04_3.jpg"},
                    },
                },
                new Product
                {
                    Name = "Ac Milan Home Socks 2021/22",
                    Description = "65% polyester, 18% polyamide, 15% cotton, 2% elastane.",
                    Price = 52.99M,
                    DiscountPrice = 12.99M,
                    InDiscount = true,
                    Availiability = true,
                    ChildCategoryId = socksId,
                    ParentCategoryId = manId,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false,
                    ProductVariants = new List<ProductVariant>
                    {
                       new ProductVariant { SizeId = 1, IsSizeAvailable = true},
                       new ProductVariant { SizeId = 2, IsSizeAvailable = true},
                       new ProductVariant { SizeId = 3, IsSizeAvailable = true},
                       new ProductVariant { SizeId = 4 , IsSizeAvailable = true},
                    },
                    Images = new List<Image>
                    {
                       new Image { Id = "211io998socks0022kk", Extention = "png"},
                    },
                }
#pragma warning restore SA1413 // Use trailing comma in multi-line initializers
            };

            await dbContext.Products.AddRangeAsync(products);
        }
    }
}
