namespace MilanWebStore.Web.ViewModels.ChildCategories
{
    using System.Linq;

    using AutoMapper;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public class ChildKitCategoryViewModel : ChildCategoryViewModel , IHaveCustomMappings
    {

        public int ProductId { get; set; }

        public int ParentCategoryId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ChildCategory, ChildKitCategoryViewModel>()
                .ForMember(x => x.ProductId, y => y.MapFrom(p => p.Products.FirstOrDefault().Id))
                .ForMember(x => x.ParentCategoryId, y => y.MapFrom(pa => pa.ParentChildCategory.FirstOrDefault().ParentCateogryId));
        }
    }
}
