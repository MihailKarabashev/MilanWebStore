namespace MilanWebStore.Web.ViewModels.Payments
{
    using System.ComponentModel.DataAnnotations;

    using static MilanWebStore.Common.ModelValidation;
    using static MilanWebStore.Common.ModelValidation.Pay;

    public class PayInputModel
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [CreditCard(ErrorMessage = CreditCardMessage)]
        [Display(Name = CardDispayName)]
        public string CardNumder { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [Display(Name = MonthDispayName)]
        public int Month { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [Display(Name = YearDispayName)]
        public int Year { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(CvcMaxLenght, MinimumLength = CvcMinLenght, ErrorMessage = CvcErrorMessage)]
        public string CVC { get; set; }

        public decimal Amount { get; set; }
    }
}
