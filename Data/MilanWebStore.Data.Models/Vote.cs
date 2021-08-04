namespace MilanWebStore.Data.Models
{
    using MilanWebStore.Data.Common.Models;

    public class Vote : BaseModel<int>
    {
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public byte Value { get; set; }
    }
}
