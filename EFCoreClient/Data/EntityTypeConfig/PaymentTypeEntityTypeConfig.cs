using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class PaymentTypeEntityTypeConfig : IEntityTypeConfiguration<PaymentType>
    {
        public void Configure(EntityTypeBuilder<PaymentType> builder)
        {
            builder.ToTable("PaymentType");

            builder.HasIndex(e => e.TypeName, "QU_PaymentType_TypeName")
                .IsUnique();

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.TypeName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
