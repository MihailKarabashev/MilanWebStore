namespace MilanWebStore.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using MilanWebStore.Web.ViewModels.Addresses;

    public interface IAddressesService
    {
        Task CreateAsync(AddressViewModel model, string userId);

        T GetUserAddress<T>(string username);
    }
}
