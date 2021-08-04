namespace MilanWebStore.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MilanWebStore.Common;
    using MilanWebStore.Data.Models;

    public class SizeConfiguration : IEntityTypeConfiguration<Size>
    {
        public void Configure(EntityTypeBuilder<Size> size)
        {
            size.Property(x => x.Name)
                .HasMaxLength(ModelValidation.Size.NameMaxLength)
                .IsRequired();
        }
    }
}
