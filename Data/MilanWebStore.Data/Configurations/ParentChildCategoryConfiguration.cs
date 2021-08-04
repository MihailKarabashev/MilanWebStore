namespace MilanWebStore.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MilanWebStore.Data.Models;

    public class ParentChildCategoryConfiguration : IEntityTypeConfiguration<ParentChildCategory>
    {
        public void Configure(EntityTypeBuilder<ParentChildCategory> parentChildCategory)
        {
            parentChildCategory
                      .Property(x => x.ChildCategoryId)
                      .IsRequired();

            parentChildCategory
                     .Property(x => x.ParentCateogryId)
                     .IsRequired();
        }
    }
}
