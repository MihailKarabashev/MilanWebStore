namespace MilanWebStore.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Services.Data.Contracts;

    public class CommentsController : AdministrationController
    {
        private readonly ICommentsService commentService;

        public CommentsController(ICommentsService commentService)
        {
            this.commentService = commentService;
        }

        public async Task<IActionResult> Delete(int id, int categoryId, int productId)
        {
            await this.commentService.RemoveAsync(id);

            return this.RedirectToAction("ById", "Products", new { area = string.Empty, id = productId , categoryId = categoryId});

        }
    }
}
