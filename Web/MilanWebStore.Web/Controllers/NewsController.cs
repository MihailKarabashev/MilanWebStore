namespace MilanWebStore.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Common;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.News;

    public class NewsController : BaseController
    {
        private readonly INewsService newsService;

        public NewsController(INewsService newsService)
        {
            this.newsService = newsService;
        }

        public IActionResult All(int id = 1)
        {
            var model = new AllNewsViewModel()
            {
                AllNews = this.newsService.GetAll<SingleNewsViewModel>(id, GlobalConstants.ItemsPerPage),
                ItemsPerPage = GlobalConstants.ItemsPerPage,
                PageNumber = id,
                ProductsCount = this.newsService.GetNewsCount(),
            };

            this.ViewData["Name"] = "News";

            return this.View(model);
        }

        public IActionResult ById(int id)
        {
            var news = this.newsService.ById<SingleNewsViewModel>(id);

            return this.View(news);
        }
    }
}
