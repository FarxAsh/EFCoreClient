using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreClient.Data.Entities
{
    public partial class Stock
    {
        public Stock()
        {
            StockRecords = new HashSet<StockRecord>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }

        public virtual ICollection<StockRecord> StockRecords { get; set; }
    }
}
