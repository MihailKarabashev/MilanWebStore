namespace MilanWebStore.Services.Data.Tests
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Data;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Data.Repositories;
    using MilanWebStore.Services.Mapping;
    using MilanWebStore.Web.ViewModels;
    using Xunit;

    public class VotesServiceTests
    {
        public VotesServiceTests()
        {
            this.InitializeMapper();
        }

        [Fact]
        public async Task VoteAsyncShouldAddVoteToDataBaseCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfRepository<Vote>(dbContext);

            var service = new VotesService(repository);

            await service.VoteAsync(1, "test", 3);
            var expected = new Vote
            {
                Id = 1,
                ApplicationUserId = "test",
                Value = 3,
            };

            var count = await dbContext.Votes.CountAsync();
            var actual = await dbContext.Votes.FirstOrDefaultAsync();

            count.Should().Be(1);
            expected.Value.Should().Be(actual.Value);

        }

        [Fact]
        public async Task GetAverageVoteShouldWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfRepository<Vote>(dbContext);

            var service = new VotesService(repository);

            await service.VoteAsync(1, "test", 3);
            await service.VoteAsync(1, "anotherTest", 3);

            var averageVote = service.GetAverageVote(1);

            averageVote.Should().Be(3);
        }


        private void InitializeMapper() => AutoMapperConfig.
          RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
    }
}
