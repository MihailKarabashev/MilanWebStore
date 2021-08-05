namespace MilanWebStore.Web.ViewModels.Administration.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using MilanWebStore.Common.ValidationAttributes;
    using MilanWebStore.Services.Mapping;

    using static MilanWebStore.Common.ModelValidation;
    using static MilanWebStore.Common.ModelValidation.Product;

    public class ProductEditViewModel : IMapFrom<MilanWebStore.Data.Models.Product>, IHaveCustomMappings
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

        public decimal PriceAfterDiscount { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [Display(Name = DiscountPriceDispayName)]
        [ComparePrice(nameof(Price))]
        public decimal? DiscountPrice { get; set; }

        [Required(ErrorMessage = ParentCategoryError)]
        [Display(Name = ParentCategoryDisplayName)]
        public int ParentCategoryId { get; set; }

        [Required(ErrorMessage = ChildCategoryError)]
        [Display(Name = ChildCategoryDisplayName)]
        public int ChildCategoryId { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = ProductVariantError)]
        [Display(Name = ProductVariantDispayName)]
        public int ProductVariantId { get; set; }

        [Required(ErrorMessage = SizeError)]
        [Display(Name = SizeDisplayName)]
        public int SizeId { get; set; }

        public IEnumerable<ProductVariantDetails> ProductVariants { get; set; }

        public IEnumerable<ProductSizeDetails> ProductSizes { get; set; }

        public IEnumerable<ProductChildCategoryDetails> ChildCategories { get; set; }

        public IEnumerable<ProductParentCategoryDetails> ParentCategories { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {

            configuration.CreateMap<MilanWebStore.Data.Models.Product, ProductEditViewModel>()
            .ForMember(x => x.PriceAfterDiscount, y => y.MapFrom(d => d.DiscountPrice != null ? d.DiscountPrice : d.Price))
            .ForMember(x => x.ImageUrl, y => y.MapFrom(i => i.Images.FirstOrDefault().RemoteUrl != null
               ? i.Images.FirstOrDefault().RemoteUrl
               : "/images/products/" + i.Images.FirstOrDefault().Id + "." + i.Images.FirstOrDefault().Extention));
        }
    }
}
