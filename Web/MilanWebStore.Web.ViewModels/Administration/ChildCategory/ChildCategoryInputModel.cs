namespace MilanWebStore.Web.ViewModels.Administration.ChildCategory
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MilanWebStore.Web.ViewModels.Administration.ParentCategory;

    using static MilanWebStore.Common.ModelValidation;
    using static MilanWebStore.Common.ModelValidation.ChildCategory;

    public class ChildCategoryInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMinLength, MinimumLength = NameMaxLength, ErrorMessage = NameLengthError)]
        public string Name { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        public int ParentCategoryId { get; set; }

        public IEnumerable<ParentCategoryViewModel> ParentCategories { get; set; }
    }
}
