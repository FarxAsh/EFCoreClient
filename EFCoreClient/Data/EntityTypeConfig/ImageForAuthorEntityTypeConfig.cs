using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class ImageForAuthorEntityTypeConfig : IEntityTypeConfiguration<ImageForAuthor>
    {
        public void Configure(EntityTypeBuilder<ImageForAuthor> builder)
        {
            builder.ToTable("ImageForAuthor");

            builder.HasIndex(e => e.AuthorId, "AK_ImageForAuthor_AuthorId")
                .IsUnique();

            builder.HasIndex(e => e.ImagePath, "UQ_ImageForAuthor_ImagePath")
                .IsUnique();

            builder.Property(e => e.ImagePath)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(d => d.Author)
                .WithOne(p => p.ImageForAuthor)
                .HasForeignKey<ImageForAuthor>(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
