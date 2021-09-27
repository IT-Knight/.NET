using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicAuthWebApi.Interfaces;
using BasicAuthWebApi.Models;

namespace BasicAuthWebApi.Data
{
    public class UserRepository : IUserRepository
    {
        private DbAppContext AppContext;

        public UserRepository(DbAppContext context)
        {
            this.AppContext = context;
        }
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await AppContext.User.ToListAsync();
        }
        
        public async Task<User> GetUser(string username)
        {
            User userExists = await AppContext.User.FirstOrDefaultAsync(u => u.Username == username);
            if (userExists == null)
            {
                return null;
            }
            return userExists;
        }

        public async Task AddUser(User user)
        {
            await AppContext.User.AddAsync(user);
        }

        public async Task Save()
        {
            await AppContext.SaveChangesAsync();
        }



        // Disposing

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    AppContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
