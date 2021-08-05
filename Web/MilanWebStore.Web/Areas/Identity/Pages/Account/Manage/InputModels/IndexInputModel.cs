namespace MilanWebStore.Web.Areas.Identity.Pages.Account.Manage.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using static MilanWebStore.Common.ModelValidation.Index;

    public class IndexInputModel
    {
        [Required]
        [Display(Name = UsernameDispayName)]
        public string Username { get; set; }

        [Required]
        [Phone(ErrorMessage = PhoneError)]
        [Display(Name = PhoneDisplayName)]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = FirstNameDispayName)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = LastNameDispayName)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = EmaileDisplayName)]
        [EmailAddress(ErrorMessage = EmailError)]
        public string Email { get; set; }
    }
}
