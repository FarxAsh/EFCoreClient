using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreClient.Data.Entities
{
    public partial class ImageForAuthor
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }
    }
}
