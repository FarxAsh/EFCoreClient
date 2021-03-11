using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class AuthorGenreEntityTypeConfig : IEntityTypeConfiguration<AuthorGenre>
    {
        public void Configure(EntityTypeBuilder<AuthorGenre> builder)
        {
            builder.HasKey(e => new { e.AuthorId, e.GenreId })
                     .HasName("PK_AuthorGenre_AuthorId_GenreId");

            builder.ToTable("AuthorGenre");

            builder.HasOne(d => d.Author)
                .WithMany(p => p.AuthorGenres)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Genre)
                .WithMany(p => p.AuthorGenres)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
