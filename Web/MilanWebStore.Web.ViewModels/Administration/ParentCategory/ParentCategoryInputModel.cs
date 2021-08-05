namespace MilanWebStore.Web.ViewModels.Administration.ParentCategory
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MilanWebStore.Web.ViewModels.Administration.ChildCategory;

    using static MilanWebStore.Common.ModelValidation;
    using static MilanWebStore.Common.ModelValidation.ParentCategory;

    public class ParentCategoryInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMinLength, MinimumLength = NameMaxLength, ErrorMessage = NameLengthError)]
        public string Name { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        public int ChildCategoryId { get; set; }

        public IEnumerable<ChildCategoryViewModel> ChildCategories { get; set; }
    }
}
