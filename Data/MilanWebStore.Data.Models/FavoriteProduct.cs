namespace MilanWebStore.Data.Models
{
    using MilanWebStore.Data.Common.Models;

    public class FavoriteProduct : BaseDeletableModel<int>
    {
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
