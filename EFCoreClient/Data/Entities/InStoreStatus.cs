using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreClient.Data.Entities
{
    public partial class InStoreStatus
    {
        public InStoreStatus()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
