using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreClient.Data.Entities
{
    public partial class BookFeedback
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime PostDate { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }

        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
    }
}
