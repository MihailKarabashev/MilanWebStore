namespace MilanWebStore.Data.Models
{
    using System.Collections.Generic;

    using MilanWebStore.Data.Common.Models;

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
