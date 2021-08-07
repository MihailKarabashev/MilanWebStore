namespace MilanWebStore.Web.ViewModels.Administration.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using MilanWebStore.Common.ValidationAttributes;
    using MilanWebStore.Web.ViewModels.Sizes;
    using static MilanWebStore.Common.ModelValidation;
    using static MilanWebStore.Common.ModelValidation.Product;

    public class ProductInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthError)]
        public string Name { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionError)]
        public string Description { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [Display(Name = PriceDispayName)]
        public decimal Price { get; set; }

        [Display(Name = DiscountPriceDispayName)]
        [ComparePrice(nameof(Price))]
        public decimal? DiscountPrice { get; set; }

        [Required(ErrorMessage = ParentCategoryError)]
        [Display(Name = ParentCategoryDisplayName)]
        public int ParentCategoryId { get; set; }

        [Required(ErrorMessage = ChildCategoryError)]
        [Display(Name = ChildCategoryDisplayName)]
        public int ChildCategoryId { get; set; }

        [Required(ErrorMessage = SizeError)]
        [Display(Name = SizeDisplayName)]
        public int[] SizeIds { get; set; }

        public IEnumerable<IFormFile> Images { get; set; }

        public IEnumerable<SizeViewModel> ProductSizes { get; set; }

        public IEnumerable<ProductChildCategoryDetails> ChildCategories { get; set; }

        public IEnumerable<ProductParentCategoryDetails> ParentCategories { get; set; }
    }
}
