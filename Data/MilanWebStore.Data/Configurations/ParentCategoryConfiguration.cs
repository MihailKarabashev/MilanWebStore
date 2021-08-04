namespace MilanWebStore.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MilanWebStore.Common;
    using MilanWebStore.Data.Models;

    public class ParentCategoryConfiguration : IEntityTypeConfiguration<ParentCategory>
    {
        public void Configure(EntityTypeBuilder<ParentCategory> parentCategory)
        {
            parentCategory.Property(x => x.Name)
                 .HasMaxLength(ModelValidation.ParentCategory.NameMaxLength)
                 .IsRequired();
        }
    }
}
