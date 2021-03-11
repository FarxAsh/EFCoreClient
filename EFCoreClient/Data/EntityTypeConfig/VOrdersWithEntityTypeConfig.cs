using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class VOrdersWithEntityTypeConfig : IEntityTypeConfiguration<VOrdersWithFullInfo>
    {
        public void Configure(EntityTypeBuilder<VOrdersWithFullInfo> builder)
        {
            builder.HasNoKey();

            builder.ToView("vOrdersWithUserInfoWithPaymentTypeWithOrderStatusWithPaymentStatus");

            builder.Property(e => e.DeleveryDate).HasColumnType("datetime");

            builder.Property(e => e.OrderDate).HasColumnType("datetime");

            builder.Property(e => e.OrderStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.PaymentStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.PaymentType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.RecieverEmail)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.RecieverFirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.RecieverLastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.RecieverPhoneNumber)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.TotalPrice).HasColumnType("money");
        }
    }
}
