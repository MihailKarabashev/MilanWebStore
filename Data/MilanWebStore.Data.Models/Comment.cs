namespace MilanWebStore.Data.Models
{
    using MilanWebStore.Data.Common.Models;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Comment : BaseDeletableModel<int>
    {
        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        public string Content { get; set; }
    }
}
