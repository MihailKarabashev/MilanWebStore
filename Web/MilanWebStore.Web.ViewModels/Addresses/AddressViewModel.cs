namespace MilanWebStore.Web.ViewModels.Addresses
{
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public class AddressViewModel : IMapFrom<Address>
    {
        public int? Id { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string ZipCode { get; set; }

        public string Notes { get; set; }
    }
}
