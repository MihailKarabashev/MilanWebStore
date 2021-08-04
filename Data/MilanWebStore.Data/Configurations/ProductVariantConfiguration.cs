namespace MilanWebStore.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MilanWebStore.Data.Models;

    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> productVariant)
        {
            productVariant
                    .Property(x => x.SizeId)
                    .IsRequired();

            productVariant
                   .Property(x => x.ProductId)
                   .IsRequired();
        }
    }
}
