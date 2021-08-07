namespace MilanWebStore.Data.Models
{
    using MilanWebStore.Data.Common.Models;
    using System.ComponentModel.DataAnnotations.Schema;

    public class FavoriteProduct : BaseDeletableModel<int>
    {
        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]

        public Product Product { get; set; }
    }
}
