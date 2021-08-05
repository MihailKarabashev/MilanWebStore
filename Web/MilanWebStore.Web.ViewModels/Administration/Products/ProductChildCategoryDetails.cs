namespace MilanWebStore.Web.ViewModels.Administration.Products
{
    using MilanWebStore.Services.Mapping;

    public class ProductChildCategoryDetails : IMapFrom<MilanWebStore.Data.Models.ChildCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
