using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreClient.Data.Entities
{
    public partial class VBooksWithFullInfo
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public string PublisherName { get; set; }
        public DateTime PublishDate { get; set; }
        public int PagesNumber { get; set; }
        public decimal BookPrice { get; set; }
        public string InStoreStatus { get; set; }
        public string Description { get; set; }
        public string ImageTitle { get; set; }
        public string ImagePath { get; set; }
    }
}
