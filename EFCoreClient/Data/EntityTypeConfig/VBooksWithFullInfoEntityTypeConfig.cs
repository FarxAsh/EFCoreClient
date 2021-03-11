using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class VBooksWithFullInfoEntityTypeConfig : IEntityTypeConfiguration<VBooksWithFullInfo>
    {
        public void Configure(EntityTypeBuilder<VBooksWithFullInfo> builder)
        {
            builder.HasNoKey();

            builder.ToView("vBookWithPublisherWithInStoreStatusWithImageInfo");

            builder.Property(e => e.BookPrice).HasColumnType("money");

            builder.Property(e => e.BookTitle)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.ImagePath).HasMaxLength(300);

            builder.Property(e => e.ImageTitle).HasMaxLength(100);

            builder.Property(e => e.InStoreStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.PublishDate).HasColumnType("datetime");

            builder.Property(e => e.PublisherName)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
