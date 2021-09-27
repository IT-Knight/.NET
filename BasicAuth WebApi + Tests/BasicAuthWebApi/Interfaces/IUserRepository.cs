using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicAuthWebApi.Models;

namespace BasicAuthWebApi.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        Task<ActionResult<IEnumerable<User>>> GetAllUsers();
        
        Task<User> GetUser(string username);

        Task AddUser(User user);

        Task Save();

    }
}
