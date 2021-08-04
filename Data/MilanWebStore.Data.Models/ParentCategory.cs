using MilanWebStore.Data.Common.Models;
using System.Collections.Generic;

namespace MilanWebStore.Data.Models
{
    public class ParentCategory : BaseDeletableModel<int>
    {
        public ParentCategory()
        {
            this.Products = new HashSet<Product>();
            this.ParentChildCategory = new HashSet<ParentChildCategory>();
        }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<ParentChildCategory> ParentChildCategory { get; set; }

    }
}
