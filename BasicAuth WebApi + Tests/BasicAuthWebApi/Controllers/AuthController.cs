using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicAuthWebApi;
using Microsoft.AspNetCore.Authorization;
using PrettyPrinter;
using BasicAuthWebApi.Data;
using System.Security.Cryptography;
using System.Text;
using BasicAuthWebApi.Interfaces;
using BasicAuthWebApi.Models;

namespace BasicAuthWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserRepository _userRepository;
        private IUserService _userService;

        public AuthController(IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        [HttpGet("[action]"), Authorize]
        public ActionResult CheckAuth()
        {
            return Ok("You successfully logged in");
        }

        // GET: api/Auth/GetUsers
        [HttpGet("[action]"), Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        // POST: api/auth/register
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(User user)
        {
            user.PrettyPrint();

            bool usernameIsBusy = await _userService.UsernameIsBusy(user.Username);

            if (usernameIsBusy) 
            {
                return Conflict(new { Error = "username exists" } );    
            }

            user.Password = _userService.HashPassword(user.Password);  // SHA512 - одинаковые пароли допускает

            await _userRepository.AddUser(user);
            await _userRepository.Save();

            return CreatedAtAction("Register", new { username = user.Username });
        }

        //[HttpPost("Login")]  // смысл логина? -> вернуть токен; создать cookie; сессию?
        //public async Task<ActionResult<User>> Login(User user)  
        //{
        //    return Ok();
        //}



    }
}
