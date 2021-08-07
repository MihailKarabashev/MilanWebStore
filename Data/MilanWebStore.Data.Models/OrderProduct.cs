namespace MilanWebStore.Data.Models
{
    using MilanWebStore.Data.Common.Models;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OrderProduct : BaseModel<int>
    {
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]

        public virtual Product Product { get; set; }

        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
