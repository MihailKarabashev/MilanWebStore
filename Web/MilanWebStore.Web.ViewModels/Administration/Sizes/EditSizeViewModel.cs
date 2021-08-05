namespace MilanWebStore.Web.ViewModels.Administration.Sizes
{
    using System.ComponentModel.DataAnnotations;

    using MilanWebStore.Services.Mapping;

    using static MilanWebStore.Common.ModelValidation;
    using static MilanWebStore.Common.ModelValidation.Size;

    public class EditSizeViewModel : IMapFrom<MilanWebStore.Data.Models.Size>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [Display(Name = SizeDispayName)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = SizeError)]
        public string Name { get; set; }
    }
}
