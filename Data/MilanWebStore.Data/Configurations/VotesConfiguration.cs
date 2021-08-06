namespace MilanWebStore.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MilanWebStore.Common;
    using MilanWebStore.Data.Models;

    public class VotesConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> vote)
        {
            vote.Property(x => x.Value)
                .HasMaxLength(ModelValidation.Vote.VoteMaxValue)
                .IsRequired();

            vote
                 .HasOne(v => v.Product)
                 .WithMany(p => p.Votes)
                 .HasForeignKey(v => v.ProductId)
                 .OnDelete(DeleteBehavior.Restrict);

            vote
                 .HasOne(v => v.ApplicationUser)
                 .WithMany(u => u.Votes)
                 .HasForeignKey(v => v.ApplicationUserId)
                 .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
