namespace MilanWebStore.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Data;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Data.Repositories;
    using MilanWebStore.Services.Mapping;
    using MilanWebStore.Web.ViewModels;
    using MilanWebStore.Web.ViewModels.News;
    using Newtonsoft.Json;
    using Xunit;
    using FluentAssertions;

    public class NewsServiceTests
    {
        public NewsServiceTests()
        {
            this.InitializeMapper();
        }

        [Fact]
        public async Task ByIdShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var news = new News { Id = 1, Title = "Test", ShortTitle = "Title", Description = "Description", CreatedOn = new DateTime(2021, 7, 21) };

            using var dbContext = new ApplicationDbContext(options);

            dbContext.News.Add(news);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<News>(dbContext);

            var service = new NewsService(repository);

            var expected = new SingleNewsViewModel
            {
                Id = 1,
                Title = "Test",
                ShortTitle = "Title",
                Description = "Description",
                CreatedOn = new DateTime(2021, 7, 21),
            };

            var actual = service.ById<SingleNewsViewModel>(expected.Id);

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact]
        public async Task GetCountShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                   .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var news = new News { Id = 1, Title = "Test", ShortTitle = "Title", Description = "Description", CreatedOn = new DateTime(2021, 7, 21) };

            using var dbContext = new ApplicationDbContext(options);

            dbContext.News.Add(news);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<News>(dbContext);

            var service = new NewsService(repository);

            var result = service.GetNewsCount();

            Assert.Equal(1, result);

        }

        [Fact]
        public async Task GetTop3LatestNewsShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                   .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var list = new List<News>()
            {
               new News { Id = 1, Title = "Test", ShortTitle = "Title1", Description = "Description1", CreatedOn = new DateTime(2021, 7, 21) },
               new News { Id = 2, Title = "Test2", ShortTitle = "Title2", Description = "Description2", CreatedOn = new DateTime(2021, 7, 22) },
               new News { Id = 3, Title = "Test3", ShortTitle = "Title3", Description = "Description3", CreatedOn = new DateTime(2021, 7, 23) },
               new News { Id = 4, Title = "Test4", ShortTitle = "Title4", Description = "Description4", CreatedOn = new DateTime(2021, 7, 24) },
               new News { Id = 5, Title = "Test5", ShortTitle = "Title5", Description = "Description5", CreatedOn = new DateTime(2021, 7, 25) },
            };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.News.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<News>(dbContext);

            var service = new NewsService(repository);

            var result = service.GetTop3LatestNews<SingleNewsViewModel>().Select(x => x.Title);
            var count = result.Count();

            var expected = list.Where(x => x.CreatedOn.Day > 22).Select(x => x.Title).ToList();

            Assert.Equal(3, count);
            expected.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetAllShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var list = new List<News>()
            {
               new News { Id = 1, Title = "Test", ShortTitle = "Title1", Description = "Description1", CreatedOn = new DateTime(2021, 7, 21) },
               new News { Id = 2, Title = "Test2", ShortTitle = "Title2", Description = "Description2", CreatedOn = new DateTime(2021, 7, 22) },
               new News { Id = 3, Title = "Test3", ShortTitle = "Title3", Description = "Description3", CreatedOn = new DateTime(2021, 7, 23) },
            };

            using var dbContext = new ApplicationDbContext(options);

            await dbContext.News.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<News>(dbContext);

            var service = new NewsService(repository);

            var result = service.GetAll<SingleNewsViewModel>(1, 6);
            var count = result.Count();

            Assert.Equal(3, count);
        }

        private void InitializeMapper() => AutoMapperConfig.
      RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
    }
}
