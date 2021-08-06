namespace MilanWebStore.Web.ViewModels.Products
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;
    using MilanWebStore.Web.ViewModels.Comments;
    using MilanWebStore.Web.ViewModels.ProductVariants;

    public class SingleProductViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public bool InDiscount { get; set; }

        public string ParentCategoryName { get; set; }

        public string ChildCategoryName { get; set; }

        public int ParentCategoryId { get; set; }

        public string SingleImageUrl { get; set; }

        public double AverageValue { get; set; }

        public int Quantity { get; set; }

        public int CommentsCount { get; set; }

        public string CommentContent { get; set; }

        public IEnumerable<string> ImageUrls { get; set; }

        public IEnumerable<ProductVariantsViewModel> ProductVariants { get; set; }

        public IEnumerable<ProductInAllViewModel> RelatedProducts { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, SingleProductViewModel>()

            .ForMember(x => x.ImageUrls, y => y.MapFrom(x => x.Images.FirstOrDefault().RemoteUrl != null
            ? x.Images.Select(x => x.RemoteUrl).ToList()
            : x.Images.Select(x => "/images/products/" + x.Id + "." + x.Extention).ToList()))

            .ForMember(x => x.SingleImageUrl, y => y.MapFrom(x => x.Images.FirstOrDefault().RemoteUrl != null
               ? x.Images.FirstOrDefault().RemoteUrl
               : "/images/products/" + x.Images.FirstOrDefault().Id + "." + x.Images.FirstOrDefault().Extention))

            .ForMember(x => x.Quantity, y => y.MapFrom(x => x.ShoppingCartProducts.FirstOrDefault().Quantity))

            .ForMember(x => x.AverageValue, y => y.MapFrom(x => x.Votes.Count() == 0 ? 0 : x.Votes.Average(x => x.Value)))

            .ForMember(x => x.CommentsCount, y => y.MapFrom(c => c.Comments.Count()));
        }
    }
}
