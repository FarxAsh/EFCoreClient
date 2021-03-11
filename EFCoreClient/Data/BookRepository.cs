using EFCoreClient.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EFCoreClient.Data
{
    public class BookRepository
    {
        private readonly BookStoreContext dbContext;

        public BookRepository(BookStoreContext context)
        {
            this.dbContext = context;
        }

        public async Task<IReadOnlyList<VBooksWithFullInfo>> GetAllBooksAsync()
        {
            try
            {
                var bookList = await dbContext.VBooksWithFullInfo.ToListAsync();
                return bookList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
