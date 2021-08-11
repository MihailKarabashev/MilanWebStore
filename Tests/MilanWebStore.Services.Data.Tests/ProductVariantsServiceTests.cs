namespace MilanWebStore.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Common;
    using MilanWebStore.Data;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Data.Repositories;
    using MilanWebStore.Services.Mapping;
    using MilanWebStore.Web.ViewModels;
    using MilanWebStore.Web.ViewModels.ProductVariants;
    using Xunit;

    public class ProductVariantsServiceTests
    {

        public ProductVariantsServiceTests()
        {
            this.InitializeMapper();
        }

        [Fact]
        public async Task AddAsyncShouldAddRightToDataBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.Products.AddAsync(new Product { Id = 1, Name = "name" });
            await dbContext.Sizes.AddAsync(new Size { Id = 1, Name = "sizeName" });

            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ProductVariant>(dbContext);
            using var productRepository = new EfDeletableEntityRepository<Product>(dbContext);
            using var sizesRepository = new EfDeletableEntityRepository<Size>(dbContext);

            var service = new ProductVariantsService(repository, productRepository, sizesRepository);

            await service.AddAsync(1, 1);

            var count = await dbContext.ProductVariants.CountAsync();

            count.Should().Be(1);
        }

        [Fact]
        public async Task AddAsyncShouldThrowNullReferenceExceptionWhenProductIdIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.Products.AddAsync(new Product { Id = 1, Name = "name" });
            await dbContext.Sizes.AddAsync(new Size { Id = 1, Name = "sizeName" });

            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ProductVariant>(dbContext);
            using var productRepository = new EfDeletableEntityRepository<Product>(dbContext);
            using var sizesRepository = new EfDeletableEntityRepository<Size>(dbContext);

            var service = new ProductVariantsService(repository, productRepository, sizesRepository);

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await service.AddAsync(1, 2));
            Assert.Equal(string.Format(ExceptionMessages.ProductNotFound, 2), exception.Message);
        }

        [Fact]
        public async Task AddAsyncShouldThrowNullReferenceExceptionWhenSizeIdIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.Products.AddAsync(new Product { Id = 1, Name = "name" });
            await dbContext.Sizes.AddAsync(new Size { Id = 1, Name = "sizeName" });

            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ProductVariant>(dbContext);
            using var productRepository = new EfDeletableEntityRepository<Product>(dbContext);
            using var sizesRepository = new EfDeletableEntityRepository<Size>(dbContext);

            var service = new ProductVariantsService(repository, productRepository, sizesRepository);

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await service.AddAsync(2, 1));
            Assert.Equal(string.Format(ExceptionMessages.SizeIdNotFoud, 2), exception.Message);
        }

        [Fact]
        public async Task RemoveAsyncShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.ProductVariants.AddAsync(new ProductVariant { Id = 1 });

            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ProductVariant>(dbContext);

            var service = new ProductVariantsService(repository, null, null);

            await service.RemoveAsync(1);

            var count = await dbContext.ProductVariants.CountAsync();

            count.Should().Be(0);
        }

        [Fact]
        public async Task RemoveAsyncShouldThrowNullReferenceExceptionWhenVariantIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.ProductVariants.AddAsync(new ProductVariant { Id = 1 });

            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ProductVariant>(dbContext);

            var service = new ProductVariantsService(repository, null, null);

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await service.RemoveAsync(2));
            Assert.Equal(string.Format(ExceptionMessages.ProductVariantIdNotFound, 2), exception.Message);
        }

        [Fact]
        public async Task GetAllVariantsShouldReturnAllProductVariantsCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);
            var product = new Product { Id = 1, Name = "Name" };
            var list = new List<ProductVariant>
            {
               new ProductVariant {Id = 1 , IsSizeAvailable = true, Size = new Size{Id =1 , Name = "S" }, ProductId = 1 },
               new ProductVariant {Id = 2 , IsSizeAvailable = true,Size = new Size{Id =2 , Name = "M" }, ProductId = 1},
               new ProductVariant {Id = 3 , IsSizeAvailable = false,Size = new Size{Id =3 , Name = "L" }, ProductId = 2},
            };

            await dbContext.ProductVariants.AddRangeAsync(list);

            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<ProductVariant>(dbContext);

            var service = new ProductVariantsService(repository, null, null);

            var result = service.GetAllVariants<ProductVariantsViewModel>(1);
            var count = result.Count();

            count.Should().Be(2);
        }

        private void InitializeMapper() => AutoMapperConfig.
           RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
    }
}
