using MilanWebStore.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MilanWebStore.Data.Models
{
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
