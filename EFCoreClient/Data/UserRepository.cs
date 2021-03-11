using EFCoreClient.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreClient.Data
{
    public class UserRepository
    {
        private readonly BookStoreContext dbContext;

        public UserRepository(BookStoreContext context)
        {
            this.dbContext = context;
        }

        public async Task<User> CreateUserAsync(User user)
        {         
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                if (user == null) throw new ArgumentNullException("Sent user in null");
                await dbContext.AddAsync(user);
                await dbContext.SaveChangesAsync();
                transaction.Commit();
                return await GetUserByEmailAsync(user.Email);    
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> IsRegisteredAsync(string email)
        {
            try
            {
                var registered = await dbContext.Users.AsNoTracking().AnyAsync(user => user.Email == email);
                return registered;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
