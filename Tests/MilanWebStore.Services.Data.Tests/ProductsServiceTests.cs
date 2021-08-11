namespace MilanWebStore.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Common;
    using MilanWebStore.Data;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Data.Repositories;
    using MilanWebStore.Services.Mapping;
    using MilanWebStore.Web.ViewModels;
    using Administration = MilanWebStore.Web.ViewModels.Administration.Products;
    using MilanWebStore.Web.ViewModels.Products;
    using Xunit;
    using System.Linq;

    public class ProductsServiceTests
    {
        private readonly string ImageExtension = "test.jpg";

        public ProductsServiceTests()
        {
            this.InitializeMapper();
        }

        [Fact]
        public async Task CreateAsyncShouldAddRightToDataBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<Product>(dbContext);

            var service = new ProductsService(repository);

            var viewModel = this.SeedInputModel(this.ImageExtension);

            await service.CreateAsync(viewModel, "path");

            var expected = this.SeedDataBaseProduct(1, "name1", 1, "cateName1", 1, "parentName1", "1", true, dbContext);


            var actual = await dbContext.Products.FirstOrDefaultAsync();
            var count = await dbContext.Products.CountAsync();

            Assert.IsType<Product>(actual);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CreateAsyncShouldThrowInvalidOperationExceptionWhenImageExtensionIsInvalid()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<Product>(dbContext);

            var service = new ProductsService(repository);

            var viewModel = this.SeedInputModel("test.pdf");

            var exception = await Assert
                 .ThrowsAsync<InvalidOperationException>(async () => await service.CreateAsync(viewModel, "path"));
            Assert.Equal(string.Format(ExceptionMessages.InvalidImageExtension, "pdf"), exception.Message);
        }

        [Fact]
        public async Task EditAsyncShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<Product>(dbContext);

            var service = new ProductsService(repository);

            var input = this.SeedInputModel(this.ImageExtension);

            await service.CreateAsync(input, "path");

            var editViewModel = new Administration.ProductEditViewModel()
            {
                Id = 1,
                Description = "NewDescription",
                Name = "DiffName",
                Price = 50,
                DiscountPrice = 25,
                ChildCategoryId = 1,
                ParentCategoryId = 1,
            };

            await service.EditAsync(editViewModel);

            var actual = await dbContext.Products.FirstOrDefaultAsync();

            Assert.Equal(editViewModel.Description, actual.Description);
            Assert.Equal(editViewModel.Name, actual.Name);
            Assert.Equal(editViewModel.Id, actual.Id);
        }

        [Fact]
        public async Task EditAsyncShouldThrowNullReferenceExceptionWhenProductIdIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<Product>(dbContext);

            var service = new ProductsService(repository);

            var editViewModel = new Administration.ProductEditViewModel() { Id = 1 };

            var exception = await Assert
                 .ThrowsAsync<NullReferenceException>(async () => await service.EditAsync(editViewModel));
            Assert.Equal(string.Format(ExceptionMessages.ProductNotFound, editViewModel.Id), exception.Message);
        }

        [Fact]
        public async Task DeleteAsyncShouldWorksCorrectlyWhenTryingToDeleteExsistingProduct()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<Product>(dbContext);

            var service = new ProductsService(repository);

            var viewModel = this.SeedInputModel(this.ImageExtension);

            await service.CreateAsync(viewModel, "path");

            await service.DeleteAsync(1);

            var count = await dbContext.Products.CountAsync();

            count.Should().Be(0);
        }

        [Fact]
        public async Task DeleteAsyncShouldThrowNullReferenceExceptionWhenProductIdIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<Product>(dbContext);

            var service = new ProductsService(repository);

            var exception = await Assert
                 .ThrowsAsync<NullReferenceException>(async () => await service.DeleteAsync(1));
            Assert.Equal(string.Format(ExceptionMessages.ProductNotFound, 1), exception.Message);
        }

        [Fact]
        public async Task GetAllShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var dbContext = new ApplicationDbContext(options);

            var list = new List<Product>
            {
               this.SeedDataBaseProduct(1, "name1", 1, "cateName1", 1, "parentName1", "1", true,dbContext),
            };


            await dbContext.Products.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Product>(dbContext);

            var service = new ProductsService(repository);

            var result = service.GetAll<Administration.ProductViewModel>(1, 6);

            var count = await dbContext.Products.CountAsync();

            count.Should().Be(1);
        }

        [Fact]
        public async Task GetTopProductsShouldReturnOnlyCountOfTopPRoducts()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            var list = new List<Product>
            {
              this.SeedDataBaseProduct(1 ,"name1",1,"cateName1",1,"parentName1","1" , true,dbContext),
              this.SeedDataBaseProduct(2 ,"name2",2,"cateName2",2,"parentName2","2" , true,dbContext),
              this.SeedDataBaseProduct(3 ,"name3",3,"cateName3",3,"parentName3","3" , true,dbContext),
              this.SeedDataBaseProduct(4 ,"name4",4,"cateName4",4,"parentName4","4" , true,dbContext),
              this.SeedDataBaseProduct(5 ,"name5",5,"cateName5",5,"parentName5","5", false,dbContext),
            };


            await dbContext.Products.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Product>(dbContext);

            var service = new ProductsService(repository);

            var result = service.GetTopProducts<Administration.ProductViewModel>();

            var count = result.Count();

            count.Should().Be(4);
        }


        private Administration.ProductInputModel SeedInputModel(string extension)
        {
            var content = "Hello World from a Fake File";
            var fileName = extension;
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            return new Administration.ProductInputModel
            {
                Id = 1,
                Name = "name1",
                Description = "Description",
                Price = 12,
                SizeIds = new[] { 1, 2 },
                DiscountPrice = 11,
                ChildCategoryId = 1,
                ParentCategoryId = 1,
                Images = new List<IFormFile> { new FormFile(stream, 0, stream.Length, "createProdcut", fileName) },
            };
        }

        private Product SeedDataBaseProduct(int id, string name, int categoryId, string categoryName, int parentId, string parentName, string imgId, bool isAvaliable, ApplicationDbContext dbContext)
        {
            var product = new Product()
            {
                Id = id,
                Name = name,
                Description = "Description",
                Price = 12,
                DiscountPrice = 11,
                Availiability = isAvaliable,
                ParentCategoryId = parentId,
                ChildCategoryId = categoryId,
                Images = new List<Image>() { new Image() { Id = imgId, Extention = "jpg" } },
            };

            if (!dbContext.ChildCategories.Any(x => x.Id == categoryId))
            {
                var childCategory = new ChildCategory() { Id = categoryId, Name = categoryName };
                product.ChildCategory = new ChildCategory() { Name = categoryName };
            }

            if (!dbContext.ParentCategories.Any(x => x.Id == parentId))
            {
                product.ParentCategory = new ParentCategory() { Name = parentName };
            }

            return product;
        }

        private void InitializeMapper() => AutoMapperConfig.
          RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
    }
}
