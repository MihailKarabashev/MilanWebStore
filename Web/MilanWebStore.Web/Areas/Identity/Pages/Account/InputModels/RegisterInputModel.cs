namespace MilanWebStore.Web.Areas.Identity.Pages.Account.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using static MilanWebStore.Common.ModelValidation;
    using static MilanWebStore.Common.ModelValidation.User;

    public class RegisterInputModel
    {
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [EmailAddress(ErrorMessage = EmailError)]
        public string Email { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(PasswordMaxLength, ErrorMessage = PasswordError, MinimumLength = PasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [DataType(DataType.Password)]
        [Display(Name = ConfirmPasswordDispayName)]
        [Compare(nameof(Password), ErrorMessage = ConfirmPasswordError)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(FirstnameMaxLength, ErrorMessage = NameLengthError, MinimumLength = FirstNameMinLength)]
        [Display(Name = FirstNameDispayName)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(LastNameMaxLength, ErrorMessage = NameLengthError, MinimumLength = LastNameMinLength)]
        [Display(Name = LastNameDispayName)]
        public string LastName { get; set; }

        public IEnumerable<IFormFile> Images { get; set; }

    }
}
