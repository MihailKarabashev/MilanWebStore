namespace MilanWebStore.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MilanWebStore.Common;
    using MilanWebStore.Data.Models;

    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> comment)
        {
            comment.Property(x => x.Content)
                .HasMaxLength(ModelValidation.Comment.ContentMaxValue)
                .IsRequired();

            comment
                .HasOne(c => c.Product)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            comment
              .HasOne(c => c.ApplicationUser)
              .WithMany(u => u.Comments)
              .HasForeignKey(c => c.ApplicationUserId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
