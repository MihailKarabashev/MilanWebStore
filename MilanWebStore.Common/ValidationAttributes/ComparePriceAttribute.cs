namespace MilanWebStore.Common.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ComparePriceAttribute : ValidationAttribute
    {
        public ComparePriceAttribute(string comparisonProperty)
        {
            this.ComparisonProperty = comparisonProperty;
        }

        public string ComparisonProperty { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var discountProperty = (decimal)value;

            var property = validationContext.ObjectType.GetProperty(this.ComparisonProperty);

            if (property == null)
            {
                throw new ArgumentException("Property with this name not found");
            }

            var comparisonValue = (decimal)property.GetValue(validationContext.ObjectInstance);

            if (discountProperty > comparisonValue)
            {
                return new ValidationResult("Discount price should not be more than price");
            }

            return ValidationResult.Success;
        }
    }
}
