namespace MilanWebStore.Data.Models
{
    using System.Collections.Generic;

    using MilanWebStore.Data.Common.Models;

    public class ShoppingCart : BaseDeletableModel<int>
    {
        public ShoppingCart()
        {
            this.ShoppingCartProducts = new HashSet<ShoppingCartProduct>();
        }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; }
    }
}
