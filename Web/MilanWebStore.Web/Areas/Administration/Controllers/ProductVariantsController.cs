namespace MilanWebStore.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Services.Data.Contracts;

    public class ProductVariantsController : AdministrationController
    {
        private readonly IProductVariantsService productVariantsService;

        public ProductVariantsController(IProductVariantsService productVariantsService)
        {
            this.productVariantsService = productVariantsService;
        }


        public async Task<IActionResult> Remove(int id, int productVariantId)
        {
            await this.productVariantsService.RemoveAsync(productVariantId);

            return this.RedirectToAction("Edit", "Products", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Add(int id, int sizeId)
        {
            await this.productVariantsService.AddAsync(sizeId, id);

            return this.RedirectToAction("Edit", "Products", new { id = id });
        }
    }
}
