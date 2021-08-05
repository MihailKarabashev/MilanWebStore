namespace MilanWebStore.Web.ViewModels.Favorites
{
    using System.Linq;

    using AutoMapper;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public class FavoriteProductViewModel : IMapFrom<FavoriteProduct>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductImageUrl { get; set; }

        public int ParentCategoryId { get; set; }

        public decimal Price { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<FavoriteProduct, FavoriteProductViewModel>()
                 .ForMember(x => x.Price, y => y
                    .MapFrom(x => x.Product.DiscountPrice != null ? x.Product.DiscountPrice : x.Product.Price))

             .ForMember(x => x.ProductImageUrl, y => y.MapFrom(x => x.Product.Images.FirstOrDefault().RemoteUrl != null
               ? x.Product.Images.FirstOrDefault().RemoteUrl
               : "/images/product/" + x.Product.Images.FirstOrDefault().Id + "." + x.Product.Images.FirstOrDefault().Extention))

             .ForMember(x => x.ParentCategoryId, y => y.MapFrom(x => x.Product.ParentCategoryId));

        }
    }
}
