namespace MilanWebStore.Services.Data.Tests
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using MilanWebStore.Common;
    using MilanWebStore.Data;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Data.Repositories;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Services.Mapping;
    using MilanWebStore.Web.ViewModels;
    using MilanWebStore.Web.ViewModels.Addresses;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class AddressesServiceTests
    {
        public AddressesServiceTests()
        {
            this.InitializeMapper();
        }

        [Theory]
        [InlineData("1", "Test1", 1, "Street1", "City1", "ZipCode1")]
        public async Task CreateShouldAddRightToDataBase(string userId, string userName, int adrressId, string street, string city, string zipCode)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfRepository<Address>(dbContext);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUserById(userId)).Returns(new ApplicationUser()
            {
                Id = userId,
                UserName = userName,
            });

            var service = new AddressesService(repository, usersServiceMock.Object);

            var expected = new AddressViewModel()
            {
                Id = adrressId,
                Street = street,
                City = city,
                ZipCode = zipCode,
            };

            await service.CreateAsync(expected, userId);

            var actual = await dbContext.Addresses
                .FirstOrDefaultAsync();

            var count = await dbContext.Addresses.CountAsync();

            Assert.Equal(1, count);
            Assert.IsType<Address>(actual);
            Assert.Equal(userId, actual.ApplicationUserId);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.City, actual.City);

        }

        [Fact]
        public async Task CreateShouldThrowExceptionIfUserIsExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using var dbContext = new ApplicationDbContext(options);

            using var repository = new EfRepository<Address>(dbContext);

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(u => u.GetUserById("1")).Returns(new ApplicationUser()
            {
                Id = "1",
                UserName = "UserName",
            });

            var address = new AddressViewModel
            {
                Id = 1,
                Street = "Street",
                City = "City",
                ZipCode = "ZipCode",
            };

            var service = new AddressesService(repository, usersServiceMock.Object);

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await service.CreateAsync(address, "2"));
            Assert.Equal(string.Format(ExceptionMessages.UserIdNotFound, "2"), exception.Message);
        }

        [Fact]
        public async Task GetUserAddressShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var user = new ApplicationUser()
            {
                Id = "1",
                UserName = "Test",
            };
            var address = new Address() { Id = 1, City = "City1", Street = "Street1", ZipCode = "ZipCode1", ApplicationUser = user };

            using var dbContext = new ApplicationDbContext(options);

            dbContext.Addresses.Add(address);
            await dbContext.SaveChangesAsync();

            using var repository = new EfRepository<Address>(dbContext);

            var service = new AddressesService(repository, null);


            var expected = new AddressViewModel()
            {
                Id = 1,
                City = "City1",
                Street = "Street1",
                ZipCode = "ZipCode1",
                ApplicationUserUserName = "Test",
            };

            var actual = service.GetUserAddress<AddressViewModel>("Test");

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        private void InitializeMapper() => AutoMapperConfig.
           RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
    }
}
