using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class StockEntityTypeConfig : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("Stock");

            builder.HasIndex(e => e.Name, "UQ_Stock_Name")
                .IsUnique();

            builder.Property(e => e.BuildingNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Street)
                .IsRequired()
                .HasMaxLength(80);
        }
    }
}
