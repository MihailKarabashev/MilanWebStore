using MilanWebStore.Data.Common.Models;

namespace MilanWebStore.Data.Models
{
    public class ShoppingCartProduct : BaseDeletableModel<int>
    {
        public int ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

    }
}
