using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreClient.Data.Entities
{
    public partial class User
    {
        public User()
        {
            BookFeedbacks = new HashSet<BookFeedback>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string FlatNumber { get; set; }

        public virtual ICollection<BookFeedback> BookFeedbacks { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
