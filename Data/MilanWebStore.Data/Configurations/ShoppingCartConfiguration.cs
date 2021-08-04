namespace MilanWebStore.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MilanWebStore.Data.Models;

    public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> shoppingCart)
        {
            shoppingCart
                   .HasOne(sc => sc.ApplicationUser)
                   .WithOne(u => u.ShoppingCart)
                   .HasForeignKey<ApplicationUser>(u => u.ShoppingCartId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
