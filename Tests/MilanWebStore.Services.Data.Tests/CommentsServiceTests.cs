using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MilanWebStore.Common;
using MilanWebStore.Data;
using MilanWebStore.Data.Models;
using MilanWebStore.Data.Repositories;
using MilanWebStore.Services.Mapping;
using MilanWebStore.Web.ViewModels;
using MilanWebStore.Web.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace MilanWebStore.Services.Data.Tests
{
    public class CommentsServiceTests
    {

        public CommentsServiceTests()
        {
            this.InitializeMapper();
        }

        [Fact]
        public async Task CreateAsyncShouldAddRightToDataBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<Comment>(dbContext);

            var service = new CommentsService(repository);

            await service.CreateAsync(1, "userName", "funnyContent");

            var count = await dbContext.Comments.CountAsync();

            count.Should().Be(1);
        }

        [Fact]
        public async Task RemoveAsyncShouldRemoveCommentFromDataBaseCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<Comment>(dbContext);

            var service = new CommentsService(repository);

            await service.CreateAsync(1, "userName", "funnyContent");

            await service.RemoveAsync(1);

            var count = await dbContext.Comments.CountAsync();

            count.Should().Be(0);
        }

        [Fact]
        public async Task RemoveAsyncShouldThrowNullReferenceExceptionWhenCommentIdIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfDeletableEntityRepository<Comment>(dbContext);

            var service = new CommentsService(repository);

            await service.CreateAsync(1, "userName", "funnyContent");

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.RemoveAsync(2));
            Assert.Equal(string.Format(ExceptionMessages.CommentNotFound, 2), exception.Message);
        }

        [Fact]
        public async Task GetAllShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            var image = new Image() { Id = "1", Extention = "jpg" };
            var user = new ApplicationUser() { Id = "1", UserName = "username", FirstName = "SM", LastName = "MS" };
            var product = new Product() { Id = 1, Name = "ProductName" };
            user.Images = new List<Image>();
            user.Images.Add(image);

            var comment = new Comment()
            {
                Id = 1,
                ApplicationUserId = user.Id,
                ProductId = product.Id,
                Content = "Funny content",
                CreatedOn = new DateTime(2021, 11, 12),
            };

            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Comment>(dbContext);

            var service = new CommentsService(repository);

            var comments = service.GetAll<CommentViewModel>(1);
            var count = comments.Count();

            count.Should().Be(1);

        }

        private void InitializeMapper() => AutoMapperConfig.
        RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
    }
}
