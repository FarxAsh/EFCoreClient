using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreClient.Data.Entities
{
    public partial class Book
    {
        public Book()
        {
            AuthorBooks = new HashSet<AuthorBook>();
            BookFeedbacks = new HashSet<BookFeedback>();
            BookGenres = new HashSet<BookGenre>();
            OrderDetails = new HashSet<OrderDetail>();
            StockRecords = new HashSet<StockRecord>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public int InStoreStatusId { get; set; }
        public int PublisherId { get; set; }
        public int PagesNumber { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public virtual InStoreStatus InStoreStatus { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ImageForBook ImageForBook { get; set; }
        public virtual ICollection<AuthorBook> AuthorBooks { get; set; }
        public virtual ICollection<BookFeedback> BookFeedbacks { get; set; }
        public virtual ICollection<BookGenre> BookGenres { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<StockRecord> StockRecords { get; set; }
    }
}
