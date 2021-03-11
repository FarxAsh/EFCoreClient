using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using EFCoreClient.Data.Entities;
using System.Reflection;

#nullable disable

namespace EFCoreClient.Data
{
    public partial class BookStoreContext : DbContext
    {
        public BookStoreContext()
        {
        }

        public BookStoreContext(DbContextOptions<BookStoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<AuthorBook> AuthorBooks { get; set; }
        public virtual DbSet<AuthorGenre> AuthorGenres { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookFeedback> BookFeedbacks { get; set; }
        public virtual DbSet<BookGenre> BookGenres { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<ImageForAuthor> ImageForAuthors { get; set; }
        public virtual DbSet<ImageForBook> ImageForBooks { get; set; }
        public virtual DbSet<InStoreStatus> InStoreStatuses { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<StockRecord> StockRecords { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VBooksWithFullInfo> VBooksWithFullInfo { get; set; }
        public virtual DbSet<VOrdersWithFullInfo> VOrdersWithFullInfo { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
