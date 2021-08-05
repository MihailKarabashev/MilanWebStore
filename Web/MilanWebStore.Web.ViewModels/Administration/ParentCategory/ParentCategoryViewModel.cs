namespace MilanWebStore.Web.ViewModels.Administration.ParentCategory
{
    using System.Linq;

    using AutoMapper;
    using MilanWebStore.Services.Mapping;

    public class ParentCategoryViewModel : IMapFrom<MilanWebStore.Data.Models.ParentCategory>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ChildCategoriesCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MilanWebStore.Data.Models.ParentCategory, ParentCategoryViewModel>()
                 .ForMember(x => x.ChildCategoriesCount, y => y.MapFrom(c => c.ParentChildCategory.Select(f => f.ChildCategory).Count()));
        }
    }
}
