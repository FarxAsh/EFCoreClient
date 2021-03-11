using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreClient.Data.Entities
{
    public partial class StockRecord
    {
        public int BookId { get; set; }
        public int StockId { get; set; }
        public int CountInStock { get; set; }

        public virtual Book Book { get; set; }
        public virtual Stock Stock { get; set; }
    }
}
