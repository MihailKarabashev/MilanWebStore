namespace MilanWebStore.Web.ViewModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    using MilanWebStore.Data.Models.Enums;
    using MilanWebStore.Web.ViewModels.Addresses;

    using static MilanWebStore.Common.ModelValidation;
    using static MilanWebStore.Common.ModelValidation.Order;

    public class OrderInputModel
    {
        public AddressViewModel Address { get; set; }

        public AddressInputModel AddressInputModel { get; set; }

        [Required(ErrorMessage = ShippingMethodError)]
        [Display(Name = ShippingMethodDispay)]
        public ShippingMethod ShippingMethod { get; set; }

        [Required(ErrorMessage = PaymentMethodError)]
        [Display(Name = PaymentMethodDispay)]
        public PaymentMethod PaymentMethod { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthError)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthError)]
        public string LastName { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
