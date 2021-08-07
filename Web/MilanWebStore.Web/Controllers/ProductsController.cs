namespace MilanWebStore.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Common;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.ChildCategories;
    using MilanWebStore.Web.ViewModels.Comments;
    using MilanWebStore.Web.ViewModels.ParentCategories;
    using MilanWebStore.Web.ViewModels.Products;
    using MilanWebStore.Web.ViewModels.ProductVariants;
    using MilanWebStore.Web.ViewModels.Sizes;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productService;
        private readonly ISizesService sizesService;
        private readonly IChildCategoriesService childCategoriesService;
        private readonly IParentCategoriesService parentCategoriesService;
        private readonly IProductVariantsService productVariantsService;
        private readonly ICommentsService commentsService;

        public ProductsController(
            IProductsService productService,
            ISizesService sizesService,
            IChildCategoriesService childCategoriesService,
            IParentCategoriesService parentCategoriesService,
            IProductVariantsService productVariantsService,
            ICommentsService commentsService)
        {
            this.productService = productService;
            this.sizesService = sizesService;
            this.childCategoriesService = childCategoriesService;
            this.parentCategoriesService = parentCategoriesService;
            this.productVariantsService = productVariantsService;
            this.commentsService = commentsService;
        }

        public IActionResult ById(int id, int categoryId)
        {
            var product = this.productService.GetById<SingleProductViewModel>(id);
            product.RelatedProducts = this.productService.GetRelatedProducts<ProductInAllViewModel>(categoryId, id);
            product.ProductVariants = this.productVariantsService.GetAllVariants<ProductVariantsViewModel>(id);
            product.Comments = this.commentsService.GetAll<CommentViewModel>(id);
            return this.View(product);
        }

        public IActionResult All([FromQuery] AllProductsQueryModel searchModel, int pageNumber = 1)
        {
            searchModel.Products = this.productService.FilterByCriteria<ProductInAllViewModel>(searchModel, pageNumber, GlobalConstants.ItemsPerPage);
            searchModel.Sizes = this.sizesService.GetAll<SizeViewModel>();
            searchModel.ChildCategories = this.childCategoriesService.GetAll<ChildCategoryViewModel>();
            searchModel.ParentCategories = this.parentCategoriesService.GetAll<ParentCategoryViewModel>();
            searchModel.Paging.ItemsPerPage = GlobalConstants.ItemsPerPage;
            searchModel.Paging.PageNumber = pageNumber;

            return this.View(searchModel);
        }
    }
}
