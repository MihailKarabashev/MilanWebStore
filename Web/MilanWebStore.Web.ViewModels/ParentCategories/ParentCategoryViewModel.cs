namespace MilanWebStore.Web.ViewModels.ParentCategories
{
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public class ParentCategoryViewModel : IMapFrom<ParentCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
