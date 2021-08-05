namespace MilanWebStore.Web.ViewModels.Administration.ChildCategory
{
    using AutoMapper;
    using MilanWebStore.Services.Mapping;

    public class ChildCategoryViewModel : IMapFrom<MilanWebStore.Data.Models.ChildCategory>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ProductsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MilanWebStore.Data.Models.ChildCategory, ChildCategoryViewModel>()
                .ForMember(x => x.ProductsCount, y => y.MapFrom(c => c.Products.Count));
        }
    }
}
