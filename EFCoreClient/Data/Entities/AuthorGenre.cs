using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreClient.Data.Entities
{
    public partial class AuthorGenre
    {
        public int AuthorId { get; set; }
        public int GenreId { get; set; }

        public virtual Author Author { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
