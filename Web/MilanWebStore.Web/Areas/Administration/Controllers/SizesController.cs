namespace MilanWebStore.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.Administration.Sizes;
    using MilanWebStore.Web.ViewModels.Sizes;

    public class SizesController : AdministrationController
    {
        private readonly ISizesService sizesService;

        public SizesController(ISizesService sizesService)
        {
            this.sizesService = sizesService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SizeInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.sizesService.CreateAsync(model);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var viewModel = new AllSizesViewModel()
            {
                Sizes = this.sizesService.GetAll<SizeViewModel>(),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.sizesService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSizeViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var size = this.sizesService.GetById<EditSizeViewModel>(model.Id);

                return this.View(size);
            }

            await this.sizesService.EditAsync(model);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Edit(int id)
        {
            var size = this.sizesService.GetById<EditSizeViewModel>(id);

            return this.View(size);
        }
    }
}
