namespace MilanWebStore.Web.ViewModels.Administration.Sizes
{
    using System.ComponentModel.DataAnnotations;

    using static MilanWebStore.Common.ModelValidation;
    using static MilanWebStore.Common.ModelValidation.Size;

    public class SizeInputModel
    {
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [Display(Name = SizeDispayName)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = SizeError)]
        public string Name { get; set; }
    }
}
