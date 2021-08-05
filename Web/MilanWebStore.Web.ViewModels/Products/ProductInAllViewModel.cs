namespace MilanWebStore.Web.ViewModels.Products
{
    using System.Linq;

    using AutoMapper;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public class ProductInAllViewModel : ProductViewModel, IHaveCustomMappings
    {
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductInAllViewModel>()
                .ForMember(x => x.ImageUrl, y => y.MapFrom(x => x.Images.FirstOrDefault().RemoteUrl != null
               ? x.Images.FirstOrDefault().RemoteUrl
               : "/images/products/" + x.Images.FirstOrDefault().Id + "." + x.Images.FirstOrDefault().Extention));
        }
    }
}
