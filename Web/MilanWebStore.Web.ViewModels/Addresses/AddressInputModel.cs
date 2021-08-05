namespace MilanWebStore.Web.ViewModels.Addresses
{
    using System.ComponentModel.DataAnnotations;

    using static MilanWebStore.Common.ModelValidation;
    using static MilanWebStore.Common.ModelValidation.Address;

    public class AddressInputModel
    {
        [Required]
        public int AddressId { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(CityMaxLenght, MinimumLength = CityMinLenght, ErrorMessage = NameLengthError)]
        [Display(Name = CityDispayName)]
        public string City { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(StreetMaxLenght, MinimumLength = StreetMinLenght, ErrorMessage = NameLengthError)]
        [Display(Name = StreeDispayName)]
        public string Street { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [Display(Name = ZipCodeDispayName)]
        public string ZipCode { get; set; }

        public string Notes { get; set; }
    }
}
