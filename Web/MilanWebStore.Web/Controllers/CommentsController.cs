namespace MilanWebStore.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Services.Data.Contracts;

    public class CommentsController : BaseController
    {
        private readonly ICommentsService commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(int productId, string commentContent, int categoryId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.commentsService.CreateAsync(productId, userId, commentContent);
            return this.RedirectToAction("ById", "Products", new { id = productId, categoryId = categoryId });
        }
    }
}
