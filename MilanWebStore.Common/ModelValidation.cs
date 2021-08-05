namespace MilanWebStore.Common
{
    public static class ModelValidation
    {
        public const string NameLengthError = "Name must be between {2} and {1} symbols";
        public const string EmptyFieldLengthError = "Please enter the field.";

        public static class Order
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 60;

            public const string ShippingMethodError = "ShippingMethod is required.";
            public const string PaymentMethodError = "PaymentMethod is required.";

            public const string PaymentMethodDispay = "Payment method";
            public const string ShippingMethodDispay = "Shipping method";

        }

        public static class Address
        {
            public const int CityMinLenght = 3;
            public const int CityMaxLenght = 30;

            public const int StreetMinLenght = 3;
            public const int StreetMaxLenght = 30;

            public const string CityDispayName = "City";
            public const string StreeDispayName = "Street";
            public const string ZipCodeDispayName = "PostCode";

        }

        public static class Pay
        {
            public const int CvcMinLenght = 3;
            public const int CvcMaxLenght = 3;

            public const int MonthFixedValue = 2;
            public const int YearFixedValue = 4;

            public const string CreditCardMessage = "Credit Card does not match credit card type";
            public const string CvcErrorMessage = "You have incorrectly entered your CVC code";
            public const string MonthErrorMessage = "The card's expiration month is invalid";
            public const string YearErrorMessage = "The card's expiration year is invalid";


            public const string MonthDispayName = "Expiration Month";
            public const string YearDispayName = "Expiration Year";
            public const string CardDispayName = "Card Number";
        }

        public static class Vote
        {
            public const int VoteMinValue = 1;
            public const int VoteMaxValue = 5;
        }

        public static class Comment
        {
            public const int ContentMaxValue = 255;
            public const int ContentMinValue = 3;
        }

        public static class ChildCategory
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 25;
        }

        public static class ParentCategory
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 25;
        }

        public static class Product
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;


            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 150;

            public const string DescriptionError = "Description must be between {2} and {1} symbols";
            public const string ParentCategoryError = "Please select gender category";
            public const string ChildCategoryError = "Please select cloth category";
            public const string SizeError = "Please select size";
            public const string ProductVariantError = "Please select product variant";


            public const string PriceDispayName = "Price";
            public const string SizeDisplayName = "Size Name";
            public const string DiscountPriceDispayName = "Discount price";
            public const string ChildCategoryDisplayName = "Cloth Category";
            public const string ProductVariantDispayName = "Product Variant";
            public const string ParentCategoryDisplayName = "Gender Category";

        }

        public static class Size
        {
            public const int NameMinLength = 1;
            public const int NameMaxLength = 10;

            public const string SizeError = "Size must be between {2} and {1} symbols";

            public const string SizeDispayName = "Size Name";
        }

        public static class News
        {
            public const int TitleMaxLenght = 30;
        }

        public static class User
        {
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;

            public const int FirstNameMinLength = 3;
            public const int FirstnameMaxLength = 25;

            public const int LastNameMinLength = 3;
            public const int LastNameMaxLength = 30;

            public const string EmailError = "Please enter valid email address";
            public const string PasswordError = "The {0} must be at least {2} and at max {1} characters long.";
            public const string ConfirmPasswordError = "The password and confirmation password do not match.";

            public const string RememberMeDispayName = "Remember me ?";
            public const string ConfirmPasswordDispayName = "Confirm password ?";
            public const string FirstNameDispayName = "First name";
            public const string LastNameDispayName = "Last name";
            public const string CurrentPasswordDispayName = "Current password";
            public const string NewPasswordDispayName = "New password";

        }

        public static class Index
        {
            public const string EmailError = "Please enter valid email address";
            public const string PhoneError = "Please enter valid phone number";

            public const string PhoneDisplayName = "Phone number";
            public const string EmaileDisplayName = "Email address";
            public const string FirstNameDispayName = "First name";
            public const string LastNameDispayName = "Last name";
            public const string UsernameDispayName = "Username";

        }
    }
}
