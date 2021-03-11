using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreClient.Data.Entities
{
    public partial class Author
    {
        public Author()
        {
            AuthorBooks = new HashSet<AuthorBook>();
            AuthorGenres = new HashSet<AuthorGenre>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }

        public virtual ImageForAuthor ImageForAuthor { get; set; }
        public virtual ICollection<AuthorBook> AuthorBooks { get; set; }
        public virtual ICollection<AuthorGenre> AuthorGenres { get; set; }
    }
}
