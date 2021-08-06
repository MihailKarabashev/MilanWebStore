namespace MilanWebStore.Web.ViewModels.ProductVariants
{
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public class ProductVariantsViewModel : IMapFrom<ProductVariant>
    {
        public int Id { get; set; }

        public int SizeId { get; set; }

        public string SizeName { get; set; }

        public string ColorName { get; set; }

        public bool IsSizeAvailable { get; set; }
    }
}
