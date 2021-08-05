namespace MilanWebStore.Web.ViewModels.Orders
{
    using System.Linq;

    using AutoMapper;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public class OrderProductViewModel : IMapFrom<OrderProduct>, IHaveCustomMappings
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<OrderProduct, OrderProductViewModel>()
                .ForMember(x => x.Price, y => y.MapFrom(p => p.Product.DiscountPrice != null ? p.Product.DiscountPrice : p.Product.Price))
                .ForMember(x => x.ImageUrl, y => y.MapFrom(x => x.Product.Images.FirstOrDefault().RemoteUrl != null
                ? x.Product.Images.FirstOrDefault().RemoteUrl
                : "/images/products/" + x.Product.Images.FirstOrDefault().Id + "." + x.Product.Images.FirstOrDefault().Extention));
        }
    }
}
