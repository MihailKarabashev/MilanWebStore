namespace MilanWebStore.Web.ViewModels.NavBars
{
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public class ParentChildNavViewModel : IMapFrom<ParentChildCategory>
    {
        public int ParentCategoryId { get; set; }

        public string ParentCategoryName { get; set; }

        public int ChildCategoryId { get; set; }

        public string ChildCategoryName { get; set; }
    }
}
