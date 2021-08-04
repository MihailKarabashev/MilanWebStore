namespace MilanWebStore.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using MilanWebStore.Common;
    using MilanWebStore.Data.Models;

    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> news)
        {
            news.Property(x => x.Description)
                .IsRequired();

            news.Property(x => x.ImageUrl)
               .IsRequired();

            news.Property(x => x.Title)
                .HasMaxLength(ModelValidation.News.TitleMaxLenght)
                .IsRequired();
        }
    }
}
