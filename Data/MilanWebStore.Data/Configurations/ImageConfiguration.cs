namespace MilanWebStore.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MilanWebStore.Data.Models;

    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> image)
        {
            image
                .HasOne(i => i.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            image
                .HasOne(i => i.ApplicationUser)
                .WithMany(u => u.Images)
                .HasForeignKey(i => i.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
