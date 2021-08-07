namespace MilanWebStore.Data.Models
{
    using MilanWebStore.Data.Common.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product : BaseDeletableModel<int>
    {
        public Product()
        {
            this.Votes = new HashSet<Vote>();
            this.Images = new HashSet<Image>();
            this.Comments = new HashSet<Comment>();
            this.OrderProducts = new HashSet<OrderProduct>();
            this.ProductVariants = new HashSet<ProductVariant>();
            this.FavoriteProducts = new HashSet<FavoriteProduct>();
            this.ShoppingCartProducts = new HashSet<ShoppingCartProduct>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public bool InDiscount { get; set; }

        public bool Availiability { get; set; }

        public int ChildCategoryId { get; set; }

        [ForeignKey(nameof(ChildCategoryId))]

        public ChildCategory ChildCategory { get; set; }

        public int ParentCategoryId { get; set; }

        [ForeignKey(nameof(ParentCategoryId))]

        public ParentCategory ParentCategory { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

        public virtual ICollection<ProductVariant> ProductVariants { get; set; }

        public virtual ICollection<FavoriteProduct> FavoriteProducts { get; set; }

        public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; }
    }
}
