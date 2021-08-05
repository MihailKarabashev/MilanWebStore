namespace MilanWebStore.Web.ViewModels.ChildCategories
{
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public class ChildCategoryViewModel : IMapFrom<ChildCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
