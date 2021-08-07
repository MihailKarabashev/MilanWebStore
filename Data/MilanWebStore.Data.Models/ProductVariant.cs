namespace MilanWebStore.Data.Models
{
    using MilanWebStore.Data.Common.Models;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProductVariant : BaseDeletableModel<int>
    {
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public int SizeId { get; set; }

        [ForeignKey(nameof(SizeId))]
        public Size Size { get; set; }

        public bool IsSizeAvailable { get; set; }
    }
}
