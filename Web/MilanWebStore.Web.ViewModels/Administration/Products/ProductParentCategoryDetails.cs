namespace MilanWebStore.Web.ViewModels.Administration.Products
{
    using MilanWebStore.Services.Mapping;

    public class ProductParentCategoryDetails : IMapFrom<MilanWebStore.Data.Models.ParentCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
