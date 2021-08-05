namespace MilanWebStore.Web.Areas.Identity.Pages.Account.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using static MilanWebStore.Common.ModelValidation;
    using static MilanWebStore.Common.ModelValidation.User;

    public class ExternalLoginInputModel
    {
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [EmailAddress(ErrorMessage = EmailError)]
        public string Email { get; set; }
    }
}
