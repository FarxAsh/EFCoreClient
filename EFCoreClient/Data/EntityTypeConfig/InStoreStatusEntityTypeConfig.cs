using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class InStoreStatusEntityTypeConfig : IEntityTypeConfiguration<InStoreStatus>
    {
        public void Configure(EntityTypeBuilder<InStoreStatus> builder)
        {
            builder.ToTable("InStoreStatus");

            builder.HasIndex(e => e.StatusName, "UQ_InStoreStatus_StatusName")
                .IsUnique();

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.StatusName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
