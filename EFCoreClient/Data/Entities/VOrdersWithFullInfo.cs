using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreClient.Data.Entities
{
    public partial class VOrdersWithFullInfo
    {
        public int Id { get; set; }
        public string RecieverFirstName { get; set; }
        public string RecieverLastName { get; set; }
        public string RecieverEmail { get; set; }
        public string RecieverPhoneNumber { get; set; }
        public string PaymentType { get; set; }
        public DateTime DeleveryDate { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
