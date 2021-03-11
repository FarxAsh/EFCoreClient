using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class OrderEntityTypeConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.Property(e => e.DeleveryDate).HasColumnType("datetime");

            builder.Property(e => e.OrderDate).HasColumnType("datetime");

            builder.Property(e => e.OrderStatusId).HasDefaultValueSql("((1))");

            builder.Property(e => e.TotalPrice).HasColumnType("money");

            builder.HasOne(d => d.OrderStatus)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.PaymentStatus)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.PaymentType)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.User)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
