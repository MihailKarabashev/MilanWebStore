namespace MilanWebStore.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MilanWebStore.Common;
    using MilanWebStore.Data.Models;

    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> address)
        {
            address.Property(x => x.City)
                .HasMaxLength(ModelValidation.Address.CityMaxLenght)
                .IsRequired();

            address.Property(x => x.Street)
                .HasMaxLength(ModelValidation.Address.StreetMaxLenght)
                .IsRequired();

            address
                .HasOne(a => a.ApplicationUser)
                .WithOne(u => u.Address)
                .HasForeignKey<Address>(x => x.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
