namespace MilanWebStore.Data.Models
{
    using MilanWebStore.Data.Common.Models;

    public class ShoppingCartProduct : BaseDeletableModel<int>
    {
        public int ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

    }
}
