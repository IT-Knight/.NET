using BasicAuth_WebApi.Interfaces;
using BasicAuth_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BasicAuth_WebApi.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)  // много repeat yourself кода? на инициализацию репозитория, и сервисов
        {
            _userRepository = userRepository;
        }

        public async Task<bool> UsernameIsBusy(string username)
        {
            return await _userRepository.GetUser(username) != null;
        }

        public async Task<bool> ValidateCredentials(string username, string password)
        {
            User user = await _userRepository.GetUser(username);

            string hashedPassword = HashPassword(user.Password);

            if (user == null || user.Password != hashedPassword) 
            { 
                return false; 
            } 
            else 
            { 
                return true; 
            }


        }

        public string HashPassword(string password)
        {
            return BitConverter.ToString(SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", ""); ;
        }
    }
}
