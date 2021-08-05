namespace MilanWebStore.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Services.Data.Contracts;

    public class DashboardController : AdministrationController
    {
        private readonly IStatisticsService statisticsService;

        public DashboardController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public IActionResult Index()
        {
            var users = this.statisticsService.GetTop5LastRegisteredUsers();

            return this.View(users);
        }

        public IActionResult CategoriesByProductsCount()
        {
            var categories = this.statisticsService.GetTop5МostProductsByChildCategories();

            return this.Json(new { JSONList = categories });
        }

        public IActionResult ProductsByCommentsCount()
        {
            var mostCommentedProducts = this.statisticsService.GetTop5MostCommentedProducts();

            return this.Json(new { JSONList = mostCommentedProducts });
        }

        public IActionResult ProductsWithMostOrders()
        {
            var bestSellingProducts = this.statisticsService.GetBestSellingProducts();

            return this.Json(new { JSONList = bestSellingProducts });
        }
    }
}
