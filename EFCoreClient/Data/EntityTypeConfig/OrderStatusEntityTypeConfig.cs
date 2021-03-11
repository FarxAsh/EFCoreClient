using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class OrderStatusEntityTypeConfig : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.ToTable("OrderStatus");

            builder.HasIndex(e => e.StatusName, "QU_OrderStatus_StatusName")
                .IsUnique();

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.StatusName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
