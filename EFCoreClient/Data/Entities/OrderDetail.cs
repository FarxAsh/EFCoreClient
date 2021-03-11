using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreClient.Data.Entities
{
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }

        public virtual Book Book { get; set; }
        public virtual Order Order { get; set; }
    }
}
