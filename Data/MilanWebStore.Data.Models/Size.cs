using MilanWebStore.Data.Common.Models;
using System.Collections.Generic;

namespace MilanWebStore.Data.Models
{
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
