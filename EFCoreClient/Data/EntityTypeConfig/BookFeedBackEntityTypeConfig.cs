using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class BookFeedBackEntityTypeConfig : IEntityTypeConfiguration<BookFeedback>
    {
        public void Configure(EntityTypeBuilder<BookFeedback> builder)
        {
            builder.ToTable("BookFeedback");

            builder.Property(e => e.Comment)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(e => e.PostDate).HasColumnType("datetime");

            builder.HasOne(d => d.Book)
                .WithMany(p => p.BookFeedbacks)
                .HasForeignKey(d => d.BookId);

            builder.HasOne(d => d.User)
                .WithMany(p => p.BookFeedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
