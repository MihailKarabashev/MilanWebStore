using MilanWebStore.Data.Models;
using MilanWebStore.Services.Mapping;

namespace MilanWebStore.Web.ViewModels.Products
{
    public class TestProduct : IMapFrom<Product>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public int ChildCategoryId { get; set; }

        public int ParentCategoryId { get; set; }
    }
}
