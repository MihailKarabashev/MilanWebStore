namespace MilanWebStore.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using MilanWebStore.Web.ViewModels.ChildCategories;
    using MilanWebStore.Web.ViewModels.News;
    using MilanWebStore.Web.ViewModels.Products;

    public class IndexViewModel
    {
        public IEnumerable<ProductInAllViewModel> ProductsInSale { get; set; }

        public IEnumerable<ChildKitCategoryViewModel> Kits { get; set; }

        public IEnumerable<ProductInAllViewModel> TopProducts { get; set; }

        public ProductOfTheWeekViewModel ProductOfTheWeek { get; set; }

        public IEnumerable<SingleNewsViewModel> LatestNews { get; set; }
    }
}
