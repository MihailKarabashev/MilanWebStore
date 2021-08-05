namespace MilanWebStore.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.Administration.ChildCategory;
    using MilanWebStore.Web.ViewModels.Administration.ParentCategory;
    using MilanWebStore.Web.ViewModels.Administration.ParentChildCategories;

    public class ChildCategoriesController : AdministrationController
    {
        private readonly IChildCategoriesService childCategoriesService;
        private readonly IParentCategoriesService parentCategoriesService;

        public ChildCategoriesController(
            IChildCategoriesService childCategoriesService,
            IParentCategoriesService parentCategoriesService)
        {
            this.childCategoriesService = childCategoriesService;
            this.parentCategoriesService = parentCategoriesService;
        }

        public IActionResult Create()
        {
            var childCategory = new ChildCategoryInputModel()
            {
                ParentCategories = this.parentCategoriesService.GetAll<ParentCategoryViewModel>(),
            };

            return this.View(childCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChildCategoryInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var childCategory = new ChildCategoryInputModel()
                {
                    ParentCategories = this.parentCategoriesService.GetAll<ParentCategoryViewModel>(),
                };

                return this.View(childCategory);
            }

            await this.childCategoriesService.CreateAsync(model);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var childCategories = new AllParentChildCategoriesViewModel()
            {
                ParentCategories = this.parentCategoriesService.GetAll<ParentCategoryViewModel>(),
                ChildCategories = this.childCategoriesService.GetAll<ChildCategoryViewModel>(),
            };

            return this.View(childCategories);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ChildCategoryViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var childCategory = this.childCategoriesService.GetById<ChildCategoryViewModel>(model.Id);

                return this.View(childCategory);
            }

            await this.childCategoriesService.EditAsync(model);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Edit(int id)
        {
            var childCategory = this.childCategoriesService.GetById<ChildCategoryViewModel>(id);

            return this.View(childCategory);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.childCategoriesService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
