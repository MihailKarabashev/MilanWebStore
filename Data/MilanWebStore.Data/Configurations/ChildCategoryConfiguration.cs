namespace MilanWebStore.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MilanWebStore.Common;
    using MilanWebStore.Data.Models;

    public class ChildCategoryConfiguration : IEntityTypeConfiguration<ChildCategory>
    {
        public void Configure(EntityTypeBuilder<ChildCategory> childCategory)
        {
            childCategory.Property(x => x.Name)
                .HasMaxLength(ModelValidation.ChildCategory.NameMaxLength)
                .IsRequired();


        }
    }
}
