namespace MilanWebStore.Web.ViewModels.Administration.ParentChildCategories
{
    using System.Collections.Generic;

    using MilanWebStore.Web.ViewModels.Administration.ChildCategory;
    using MilanWebStore.Web.ViewModels.Administration.ParentCategory;

    public class AllParentChildCategoriesViewModel
    {
        public IEnumerable<ParentCategoryViewModel> ParentCategories { get; set; }

        public IEnumerable<ChildCategoryViewModel> ChildCategories { get; set; }

    }
}
