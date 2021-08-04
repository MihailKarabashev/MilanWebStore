namespace MilanWebStore.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MilanWebStore.Data.Models;

    public class FavoriteProductConfiguration : IEntityTypeConfiguration<FavoriteProduct>
    {
        public void Configure(EntityTypeBuilder<FavoriteProduct> favoriteProduct)
        {
            favoriteProduct
                    .Property(x => x.ApplicationUserId)
                    .IsRequired();

            favoriteProduct
                    .Property(x => x.ProductId)
                    .IsRequired();
        }
    }
}
