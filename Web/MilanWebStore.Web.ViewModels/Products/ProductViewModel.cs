namespace MilanWebStore.Web.ViewModels.Products
{
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public abstract class ProductViewModel : IMapFrom<Product>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public bool InDiscount { get; set; }

        public string ImageUrl { get; set; }

        public int ParentCategoryId { get; set; }

        public string ChildCategoryName { get; set; }

        public int ChildCategoryId { get; set; }

        public int SizeId { get; set; }
    }
}
