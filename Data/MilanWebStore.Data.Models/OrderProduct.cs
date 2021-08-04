namespace MilanWebStore.Data.Models
{
    using MilanWebStore.Data.Common.Models;

    public class OrderProduct : BaseModel<int>
    {
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
