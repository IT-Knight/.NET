using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicAuth_WebApi;
using BasicAuth_WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using PrettyPrinter;
using BasicAuth_WebApi.Interfaces;
using BasicAuth_WebApi.Data;
using System.Security.Cryptography;
using System.Text;

namespace BasicAuth_WebApi.Controllers
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

        // GET: api/Auth
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

        //[HttpPost("Login")]  // смысл логина?
        //public async Task<ActionResult<User>> Login(User user)  // session login by basic
        //{
        //    return Ok();
        //}



    }
}
