namespace MilanWebStore.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.Administration.ChildCategory;
    using MilanWebStore.Web.ViewModels.Administration.ParentCategory;
    using MilanWebStore.Web.ViewModels.Administration.ParentChildCategories;

    public class ParentCategoriesController : AdministrationController
    {
        private readonly IParentCategoriesService parentCategoriesService;
        private readonly IChildCategoriesService childCategoriesService;

        public ParentCategoriesController(
            IParentCategoriesService parentCategoriesService,
            IChildCategoriesService childCategoriesService)
        {
            this.parentCategoriesService = parentCategoriesService;
            this.childCategoriesService = childCategoriesService;
        }

        public IActionResult Create()
        {
            var parentCategory = new ParentCategoryInputModel()
            {
                ChildCategories = this.childCategoriesService.GetAll<ChildCategoryViewModel>(),
            };

            return this.View(parentCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ParentCategoryInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var parentCategory = new ParentCategoryInputModel()
                {
                    ChildCategories = this.childCategoriesService.GetAll<ChildCategoryViewModel>(),
                };

                return this.View(parentCategory);
            }

            await this.parentCategoriesService.CreateAsync(model);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var parentChildCategoriesModel = new AllParentChildCategoriesViewModel()
            {
                ParentCategories = this.parentCategoriesService.GetAll<ParentCategoryViewModel>(),
                ChildCategories = this.childCategoriesService.GetAll<ChildCategoryViewModel>(),
            };

            return this.View(parentChildCategoriesModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ParentCategoryViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var parent = this.parentCategoriesService.GetById<ParentCategoryViewModel>(model.Id);
                return this.View(parent);
            }

            await this.parentCategoriesService.EditAsync(model);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Edit(int id)
        {
            var parent = this.parentCategoriesService.GetById<ParentCategoryViewModel>(id);

            return this.View(parent);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.parentCategoriesService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
