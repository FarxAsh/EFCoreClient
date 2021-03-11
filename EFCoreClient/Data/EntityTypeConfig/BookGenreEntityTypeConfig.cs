using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class BookGenreEntityTypeConfig : IEntityTypeConfiguration<BookGenre>
    {
        public void Configure(EntityTypeBuilder<BookGenre> builder)
        {
            builder.HasKey(e => new { e.BookId, e.GenreId })
                    .HasName("PK_BookGenre_BookId_GenreId");

            builder.ToTable("BookGenre");

            builder.HasOne(d => d.Book)
                .WithMany(p => p.BookGenres)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Genre)
                .WithMany(p => p.BookGenres)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
