namespace MilanWebStore.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using MilanWebStore.Common;
    using MilanWebStore.Data.Common.Repositories;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Services.Mapping;
    using MilanWebStore.Web.ViewModels.Addresses;

    public class AddressesService : IAddressesService
    {
        private readonly IRepository<Address> addressesRepository;
        private readonly IUsersService usersService;

        public AddressesService(
            IRepository<Address> addressesRepository,
            IUsersService usersService)
        {
            this.addressesRepository = addressesRepository;
            this.usersService = usersService;
        }

        public async Task CreateAsync(AddressViewModel model, string userId)
        {
            var user = this.usersService.GetUserById(userId);

            if (user == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.UserIdNotFound, userId));
            }

            var address = new Address()
            {
                City = model.City,
                Street = model.Street,
                ZipCode = model.ZipCode,
                Notes = model.Notes,
                ApplicationUserId = userId,
            };

            await this.addressesRepository.AddAsync(address);
            await this.addressesRepository.SaveChangesAsync();

            user.AddressId = address.Id;
        }

        public T GetUserAddress<T>(string username)
        {
            return this.addressesRepository.All().Where(x =>
            x.ApplicationUser.UserName == username).To<T>().FirstOrDefault();
        }
    }
}
