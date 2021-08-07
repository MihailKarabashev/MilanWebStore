namespace MilanWebStore.Data.Models
{
    using MilanWebStore.Data.Common.Models;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ShoppingCartProduct : BaseDeletableModel<int>
    {
        public int ShoppingCartId { get; set; }

        [ForeignKey(nameof(ShoppingCartId))]
        public ShoppingCart ShoppingCart { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public int Quantity { get; set; }

    }
}
