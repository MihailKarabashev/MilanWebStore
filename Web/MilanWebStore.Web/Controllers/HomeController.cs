namespace MilanWebStore.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels;
    using MilanWebStore.Web.ViewModels.ChildCategories;
    using MilanWebStore.Web.ViewModels.Home;
    using MilanWebStore.Web.ViewModels.News;
    using MilanWebStore.Web.ViewModels.Products;

    public class HomeController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly IChildCategoriesService childCategoriesService;
        private readonly INewsService newsService;

        public HomeController(
            IProductsService productsService,
            IChildCategoriesService childCategoriesService,
            INewsService newsService)
        {
            this.productsService = productsService;
            this.childCategoriesService = childCategoriesService;
            this.newsService = newsService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
                ProductsInSale = this.productsService.GetAllInSale<ProductInAllViewModel>(),
                Categories = this.childCategoriesService.GetAllKits<ChildCategoryViewModel>(),
                TopProducts = this.productsService.GetTopProducts<ProductInAllViewModel>(),
                //ProductOfTheWeek = this.productsService.DealOfTheWeek(),
                LatestNews = this.newsService.GetTop3LatestNews<SingleNewsViewModel>(),
            };

            return this.View(viewModel);
        }

        public IActionResult Contacts()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult Faq()
        {
            return this.View();
        }

        public IActionResult NotFound404()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
