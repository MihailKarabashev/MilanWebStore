namespace MilanWebStore.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MilanWebStore.Data.Models;

    public class OrderProductsConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> orderProduct)
        {
            orderProduct
            .Property(x => x.ProductId)
            .IsRequired();

            orderProduct
            .Property(x => x.OrderId)
            .IsRequired();

            orderProduct
                .Property(x => x.Price)
                .IsRequired();

            orderProduct
                .Property(x => x.Quantity)
                .IsRequired();
        }
    }
}
