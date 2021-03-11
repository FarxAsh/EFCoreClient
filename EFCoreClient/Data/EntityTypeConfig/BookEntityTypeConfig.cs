using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class BookEntityTypeConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Book");

            builder.HasIndex(e => new { e.Title, e.PublisherId }, "AK_Book_Title_PublisherId")
                .IsUnique();

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.InStoreStatusId).HasDefaultValueSql("((1))");

            builder.Property(e => e.Price).HasColumnType("money");

            builder.Property(e => e.PublishDate).HasColumnType("datetime");

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasOne(d => d.InStoreStatus)
                .WithMany(p => p.Books)
                .HasForeignKey(d => d.InStoreStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
