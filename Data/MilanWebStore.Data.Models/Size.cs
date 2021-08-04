namespace MilanWebStore.Data.Models
{
    using System.Collections.Generic;

    using MilanWebStore.Data.Common.Models;

    public class Size : BaseDeletableModel<int>
    {
        public Size()
        {
            this.ProductVariants = new HashSet<ProductVariant>();
        }

        public string Name { get; set; }

        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
