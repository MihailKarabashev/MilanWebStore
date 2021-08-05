namespace MilanWebStore.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Common;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.Administration.Products;
    using MilanWebStore.Web.ViewModels.Sizes;

    public class ProductsController : AdministrationController
    {
        private readonly IChildCategoriesService childCategoriesService;
        private readonly IParentCategoriesService parentCategoriesService;
        private readonly IProductsService productsService;
        private readonly IWebHostEnvironment environment;
        private readonly ISizesService sizesService;

        public ProductsController(
            IChildCategoriesService childCategoriesService,
            IParentCategoriesService parentCategoriesService,
            IProductsService productsService,
            IWebHostEnvironment environment,
            ISizesService sizesService)
        {
            this.childCategoriesService = childCategoriesService;
            this.parentCategoriesService = parentCategoriesService;
            this.productsService = productsService;
            this.environment = environment;
            this.sizesService = sizesService;
        }

        public IActionResult Create()
        {
            var product = new ProductInputModel()
            {
                ChildCategories = this.childCategoriesService.GetAll<ProductChildCategoryDetails>(),
                ParentCategories = this.parentCategoriesService.GetAll<ProductParentCategoryDetails>(),
                ProductSizes = this.sizesService.GetAll<SizeViewModel>(),
            };

            return this.View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var product = new ProductInputModel()
                {
                    ChildCategories = this.childCategoriesService.GetAll<ProductChildCategoryDetails>(),
                    ParentCategories = this.parentCategoriesService.GetAll<ProductParentCategoryDetails>(),
                    ProductSizes = this.sizesService.GetAll<SizeViewModel>(),
                };

                return this.View(product);
            }

            await this.productsService.CreateAsync(model, $"{this.environment.WebRootPath}/images");

            this.TempData["SuccessfullyAdded"] = "Product added successfully.";

            return this.RedirectToAction(nameof(this.All));
        }


        public IActionResult All(int id = 1)
        {
            var products = new AllProductsViewModel()
            {
                Products = this.productsService.GetAll<ProductViewModel>(id, GlobalConstants.ItemsPerPage),
                ItemsPerPage = GlobalConstants.ItemsPerPage,
                PageNumber = id,
                ProductsCount = this.productsService.GetProductsCount(),
            };

            this.ViewData["Name"] = "Products";

            return this.View(products);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.productsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.All));
        }


        public IActionResult Edit(int id)
        {
            var product = this.productsService.GetById<ProductEditViewModel>(id);
            product.ParentCategories = this.parentCategoriesService.GetAll<ProductParentCategoryDetails>();
            product.ChildCategories = this.childCategoriesService.GetAll<ProductChildCategoryDetails>();
            product.ProductSizes = this.sizesService.GetAll<SizeViewModel>();

            return this.View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var product = this.productsService.GetById<ProductEditViewModel>(model.Id);
                product.ParentCategories = this.parentCategoriesService.GetAll<ProductParentCategoryDetails>();
                product.ChildCategories = this.childCategoriesService.GetAll<ProductChildCategoryDetails>();
                product.ProductSizes = this.sizesService.GetAll<SizeViewModel>();

                return this.View(product);
            }

            await this.productsService.EditAsync(model);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
