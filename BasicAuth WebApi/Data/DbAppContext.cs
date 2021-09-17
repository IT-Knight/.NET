using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BasicAuth_WebApi.Models;

namespace BasicAuth_WebApi
{
    public class DbAppContext : DbContext
    {
        public DbAppContext (DbContextOptions<DbAppContext> options)
            : base(options)
        {
        }

        public DbSet<BasicAuth_WebApi.Models.User> User { get; set; }
    }
}
