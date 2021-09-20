using BasicAuth_WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicAuth_WebApi.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        Task<ActionResult<IEnumerable<User>>> GetAllUsers();
        
        Task<User> GetUser(string username);

        Task AddUser(User user);

        Task Save();

    }
}
