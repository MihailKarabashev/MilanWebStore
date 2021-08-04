namespace MilanWebStore.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MilanWebStore.Data.Models;

    public class ShoppingCartProductsConfiguration : IEntityTypeConfiguration<ShoppingCartProduct>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartProduct> shoppingCartProduct)
        {
            shoppingCartProduct
                .Property(x => x.ShoppingCartId)
                .IsRequired();

            shoppingCartProduct
                 .Property(x => x.ProductId)
                 .IsRequired();
        }
    }
}
