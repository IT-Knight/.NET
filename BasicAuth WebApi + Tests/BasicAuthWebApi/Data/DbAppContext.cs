using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BasicAuthWebApi.Models;

namespace BasicAuthWebApi
{
    public class DbAppContext : DbContext
    {
        public DbAppContext (DbContextOptions<DbAppContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}
