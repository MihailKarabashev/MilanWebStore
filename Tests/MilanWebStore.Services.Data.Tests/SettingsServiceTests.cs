namespace MilanWebStore.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MilanWebStore.Data;
    using MilanWebStore.Data.Common.Repositories;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Data.Repositories;

    using Microsoft.EntityFrameworkCore;

    using Moq;

    using Xunit;

    public class SettingsServiceTests
    {
        [Fact]
        public void GetCountShouldReturnCorrectNumber()
        {
            var repository = new Mock<IDeletableEntityRepository<Setting>>();

            repository.Setup(r => r.All()).Returns(new List<Setting>
                                                        {
                                                            new Setting(),
                                                            new Setting(),
                                                            new Setting(),
                                                        }.AsQueryable());

            var service = new SettingsService(repository.Object);

            Assert.Equal(3, service.GetCount());

            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task GetCountShouldReturnCorrectNumberUsingDbContext()
        {
            // Правим опции за създаване
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SettingsTestDb").Options;

            // създаваме базата
            using var dbContext = new ApplicationDbContext(options);

            //добавяме към базата
            dbContext.Settings.Add(new Setting());
            dbContext.Settings.Add(new Setting());
            dbContext.Settings.Add(new Setting());

            //сейфаме
            await dbContext.SaveChangesAsync();

            //създаваме репосизитори от тип делитаблеРепозитори от сеттинг
            using var repository = new EfDeletableEntityRepository<Setting>(dbContext);

            // правим инстанция на сървис класа -> сеттингс сървис и му подаваме репозитори
            var service = new SettingsService(repository);

            //проверка
            Assert.Equal(3, service.GetCount());
        }
    }
}
