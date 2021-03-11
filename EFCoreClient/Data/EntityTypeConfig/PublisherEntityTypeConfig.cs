using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class PublisherEntityTypeConfig : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.ToTable("Publisher");

            builder.HasIndex(e => e.Name, "AK_Publisher_Name")
                .IsUnique();

            builder.HasIndex(e => e.Email, "UQ_Publisher_Email")
                .IsUnique();

            builder.HasIndex(e => e.PhoneNumber, "UQ_Publisher_PhoneNumber")
                .IsUnique();

            builder.Property(e => e.BriefInfo).HasMaxLength(500);

            builder.Property(e => e.BuildingNumber).HasMaxLength(20);

            builder.Property(e => e.City).HasMaxLength(80);

            builder.Property(e => e.Country).HasMaxLength(80);

            builder.Property(e => e.Email).HasMaxLength(100);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Street).HasMaxLength(80);
        }
    }
}
