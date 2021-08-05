namespace MilanWebStore.Web.ViewModels.Orders
{
    using System;

    using AutoMapper;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public class MyOrderViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string OrderStatus { get; set; }

        public string PaymentMethod { get; set; }

        public string ShippingMethod { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime CreatedOn { get; set; }

        public string AddressStreet { get; set; }

        public string AddressZipCode { get; set; }

        public string AddressCity { get; set; }

        public string ApplicationUserUserName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, MyOrderViewModel>()
                .ForMember(x => x.OrderStatus, y => y.MapFrom(p => p.OrderStatus.ToString()))
                .ForMember(x => x.PaymentMethod, y => y.MapFrom(p => p.PaymentMethod.ToString()))
                .ForMember(x => x.ShippingMethod, y => y.MapFrom(p => p.ShippingMethod.ToString()));
        }
    }
}
