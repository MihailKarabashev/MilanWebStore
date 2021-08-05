namespace MilanWebStore.Web.Areas.Identity.Pages.Account.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using static MilanWebStore.Common.ModelValidation;
    using static MilanWebStore.Common.ModelValidation.User;

    public class LoginInputModel
    {
        [Required(ErrorMessage = EmptyFieldLengthError)]
        public string Username { get; set; }

        [Required(ErrorMessage =EmptyFieldLengthError)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = RememberMeDispayName)]
        public bool RememberMe { get; set; }
    }
}
