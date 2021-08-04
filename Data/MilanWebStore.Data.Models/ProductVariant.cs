namespace MilanWebStore.Data.Models
{
    using MilanWebStore.Data.Common.Models;

    public class ProductVariant : BaseDeletableModel<int>
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int SizeId { get; set; }

        public Size Size { get; set; }

        public bool IsSizeAvailable { get; set; }
    }
}
