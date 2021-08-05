namespace MilanWebStore.Web.ViewModels.Administration.Products
{
    using System.Linq;

    using AutoMapper;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public class ProductViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string DiscountPrice { get; set; }

        public string Availiability { get; set; }

        public int ParentCategoryId { get; set; }

        public string ChildCategoryName { get; set; }

        public string ParentCategoryName { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductViewModel>()
                .ForMember(x => x.ChildCategoryName, y => y.MapFrom(c => c.ChildCategory.Name))
                .ForMember(x => x.ParentCategoryName, y => y.MapFrom(p => p.ParentCategory.Name))
                .ForMember(x => x.DiscountPrice, y => y.MapFrom(d => d.DiscountPrice != null ? d.DiscountPrice.ToString() : "N/A"))
                .ForMember(x => x.Availiability, y => y.MapFrom(a => a.Availiability == true ? "YES" : "NO"))
                .ForMember(x => x.ImageUrl, y => y.MapFrom(i => i.Images.FirstOrDefault().RemoteUrl != null
               ? i.Images.FirstOrDefault().RemoteUrl
               : "/images/products/" + i.Images.FirstOrDefault().Id + "." + i.Images.FirstOrDefault().Extention));
        }
    }
}
