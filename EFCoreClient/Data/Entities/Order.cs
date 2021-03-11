using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreClient.Data.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int PaymentTypeId { get; set; }
        public DateTime DeleveryDate { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderStatusId { get; set; }
        public int PaymentStatusId { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }
        public virtual PaymentStatus PaymentStatus { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
