namespace MilanWebStore.Web.Areas.Identity.Pages.Account.Manage.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using static MilanWebStore.Common.ModelValidation;
    using static MilanWebStore.Common.ModelValidation.User;

    public class ChangePasswordInputModel
    {
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [DataType(DataType.Password)]
        [Display(Name = CurrentPasswordDispayName)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(PasswordMaxLength, ErrorMessage = PasswordError, MinimumLength = PasswordMinLength)]
        [DataType(DataType.Password)]
        [Display(Name = NewPasswordDispayName)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [DataType(DataType.Password)]
        [Display(Name = ConfirmPasswordDispayName)]
        [Compare(nameof(NewPassword), ErrorMessage = ConfirmPasswordError)]
        public string ConfirmPassword { get; set; }
    }
}
