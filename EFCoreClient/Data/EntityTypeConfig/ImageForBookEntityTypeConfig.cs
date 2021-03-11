using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class ImageForBookEntityTypeConfig : IEntityTypeConfiguration<ImageForBook>
    {
        public void Configure(EntityTypeBuilder<ImageForBook> builder)
        {
            builder.ToTable("ImageForBook");

            builder.HasIndex(e => e.BookId, "AK_ImageForBook_BookId")
                .IsUnique();

            builder.HasIndex(e => e.ImagePath, "UQ_ImageForBook_ImagePath")
                .IsUnique();

            builder.Property(e => e.ImagePath)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(d => d.Book)
                .WithOne(p => p.ImageForBook)
                .HasForeignKey<ImageForBook>(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
