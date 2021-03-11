using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class StockRecordEntityTypeConfig : IEntityTypeConfiguration<StockRecord>
    {
        public void Configure(EntityTypeBuilder<StockRecord> builder)
        {
            builder.HasKey(e => new { e.BookId, e.StockId })
                   .HasName("PK_StockRecord_BookId_StockId");

            builder.ToTable("StockRecord");

            builder.HasOne(d => d.Book)
                .WithMany(p => p.StockRecords)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Stock)
                .WithMany(p => p.StockRecords)
                .HasForeignKey(d => d.StockId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
