namespace MilanWebStore.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MilanWebStore.Common;
    using MilanWebStore.Data.Models;

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> product)
        {
            product.Property(x => x.Name)
                .HasMaxLength(ModelValidation.Product.NameMaxLength)
                .IsRequired();

            product.Property(x => x.Description)
                .HasMaxLength(ModelValidation.Product.DescriptionMaxLength)
                .IsRequired();

            product
                .HasOne(p => p.ChildCategory)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ChildCategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            product
                .HasOne(p => p.ParentCategory)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ParentCategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
