namespace MilanWebStore.Web.ViewModels.Orders
{
    using AutoMapper;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Data.Models.Enums;
    using MilanWebStore.Services.Mapping;

    public class CompleteOrderViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string AddressCity { get; set; }

        public string AddressStreet { get; set; }

        public string AddressNotes { get; set; }

        public string AddressZipCode { get; set; }

        public string PaymentTypeName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, CompleteOrderViewModel>()
                .ForMember(x => x.FullName, y => y.MapFrom(x => x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName))
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(x => x.ApplicationUser.PhoneNumber))
                .ForMember(x => x.AddressCity, y => y.MapFrom(x => x.Address.City))
                .ForMember(x => x.PaymentTypeName, y => y.MapFrom(x => x.PaymentMethod.ToString()));
        }
    }
}
