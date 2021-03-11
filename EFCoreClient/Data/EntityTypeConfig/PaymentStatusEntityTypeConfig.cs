using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class PaymentStatusEntityTypeConfig : IEntityTypeConfiguration<PaymentStatus>
    {
        public void Configure(EntityTypeBuilder<PaymentStatus> builder)
        {
            builder.ToTable("PaymentStatus");

            builder.HasIndex(e => e.StatusName, "QU_PaymentStatus_StatusName")
                .IsUnique();

            builder.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            builder.Property(e => e.StatusName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
