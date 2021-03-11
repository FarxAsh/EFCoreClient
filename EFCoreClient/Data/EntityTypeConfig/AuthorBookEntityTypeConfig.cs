using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class AuthorBookEntityTypeConfig : IEntityTypeConfiguration<AuthorBook>
    {
        public void Configure(EntityTypeBuilder<AuthorBook> builder)
        {
            builder.HasKey(e => new { e.AuthorId, e.BookId })
                     .HasName("PK_AuthorBook_AuthorId_BookId");

            builder.ToTable("AuthorBook");

            builder.HasOne(d => d.Author)
                .WithMany(p => p.AuthorBooks)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Book)
                .WithMany(p => p.AuthorBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
