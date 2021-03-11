using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreClient.Data.Entities
{
    public partial class Genre
    {
        public Genre()
        {
            AuthorGenres = new HashSet<AuthorGenre>();
            BookGenres = new HashSet<BookGenre>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AuthorGenre> AuthorGenres { get; set; }
        public virtual ICollection<BookGenre> BookGenres { get; set; }
    }
}
