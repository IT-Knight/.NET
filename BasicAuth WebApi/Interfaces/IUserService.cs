using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicAuth_WebApi.Interfaces
{
    public interface IUserService
    {
        Task<bool> ValidateCredentials(string username, string password);

        string HashPassword(string password);

        Task<bool> UsernameIsBusy(string username);
    }
}
