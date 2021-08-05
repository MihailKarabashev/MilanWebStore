namespace MilanWebStore.Web.ViewModels.ShoppingCarts
{
    using System.Linq;

    using AutoMapper;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public class ShoppingCartProductsViewModel : IMapFrom<ShoppingCartProduct>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string ShoppingCartApplicationUserId { get; set; }

        public int ProductId { get; set; }

        public string ProductImageUrl { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice => this.Quantity * this.Price;

        public int ProductParentCategoryId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ShoppingCartProductsViewModel>()
                 .ForMember(x => x.Price, y => y.MapFrom(x => x.DiscountPrice == null ? x.Price : x.DiscountPrice))

                 .ForMember(x => x.ProductImageUrl, y => y.MapFrom(x => x.Images.FirstOrDefault().RemoteUrl != null
               ? x.Images.FirstOrDefault().RemoteUrl
               : "/images/products/" + x.Images.FirstOrDefault().Id + "." + x.Images.FirstOrDefault().Extention))

                .ForMember(x => x.ProductName, y => y.MapFrom(x => x.Name))

                .ForMember(x => x.ProductParentCategoryId, y => y.MapFrom(x => x.ParentCategoryId));
        }
    }
 }
