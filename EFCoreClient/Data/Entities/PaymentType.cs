using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreClient.Data.Entities
{
    public partial class PaymentType
    {
        public PaymentType()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
