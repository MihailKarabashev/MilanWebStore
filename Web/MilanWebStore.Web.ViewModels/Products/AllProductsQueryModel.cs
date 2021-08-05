namespace MilanWebStore.Web.ViewModels.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MilanWebStore.Web.ViewModels.Administration.ParentCategory;
    using MilanWebStore.Web.ViewModels.ChildCategories;
    using MilanWebStore.Web.ViewModels.Sizes;

    public class AllProductsQueryModel
    {
        [Display(Name = "Cloth Categories")]
        public int? ParentCategoryId { get; set; }

        [Display(Name = "Gender Categories")]
        public int? ChildCategoryId { get; set; }

        [Display(Name = "Sizes")]
        public int? SizeId { get; set; }

        public string SearchTerm { get; set; }

        public PagingViewModel Paging { get; set; }

        [Display(Name = "Price Range")]
        public ProductPriceFilterViewModel FilterByPrice { get; set; }

        public IEnumerable<SizeViewModel> Sizes { get; set; }

        public IEnumerable<ProductInAllViewModel> Products { get; set; }

        public IEnumerable<ParentCategoryViewModel> ParentCategories { get; set; }

        public IEnumerable<ChildCategoryViewModel> ChildCategories { get; set; }
    }
}
