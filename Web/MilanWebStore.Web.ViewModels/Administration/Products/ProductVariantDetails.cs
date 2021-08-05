namespace MilanWebStore.Web.ViewModels.Administration.Products
{
    using AutoMapper;
    using MilanWebStore.Services.Mapping;

    public class ProductVariantDetails : IMapFrom<MilanWebStore.Data.Models.ProductVariant>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MilanWebStore.Data.Models.ProductVariant, ProductVariantDetails>()
                .ForMember(x => x.Name, y => y.MapFrom(pv => pv.Product.Name + "/" + pv.Size.Name));
        }
    }
}
