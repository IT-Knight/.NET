using BasicAuthWebApi.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PrettyPrinter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BasicAuthWebApi.Interfaces;
using BasicAuthWebApi.Models;

namespace BasicAuthWebApi.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private IUserRepository _userRepository;
        private IUserService _userService;

        public AuthMiddleware(RequestDelegate next, IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)  // надо async, иначе DbContext/UserRepository не фурычет, не по дефолту
        {                                                                  
            string authHeader = httpContext.Request.Headers["Authorization"];
            authHeader.PrettyPrint();
            
            if (authHeader != null)
            {
                string auth = authHeader.Split(" ")[1];
                auth.PrettyPrint();
                Convert.FromBase64String(auth).PrettyPrint();
                var auth_data = Encoding.UTF8.GetString(Convert.FromBase64String(auth));
                auth_data.PrettyPrint();
                string username = auth_data.Split(":")[0];

                string password = auth_data.Split(":")[1];
               
                //////// Pass Validator
                SHA512 sha512hash = SHA512.Create();  // 1
                sha512hash.PrettyPrint();
                
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);  // 2
                passwordBytes.PrettyPrint();

                byte[] hashBytes = sha512hash.ComputeHash(passwordBytes);  // 3
                hashBytes.PrettyPrint();

                BitConverter.ToString(hashBytes).PrettyPrint();
                password = BitConverter.ToString(hashBytes).Replace("-", ""); // 4

                ("" == String.Empty).PrettyPrint();

                User user = await _userRepository.GetUser(username);  // 5
                
                if (user == null)
                {
                    httpContext.Response.StatusCode = 401;
                    return;
                }
                else if (user.Password != password)
                {
                    httpContext.Response.StatusCode = 403;
                    
                    httpContext.Response.Body.Write(JsonSerializer.SerializeToUtf8Bytes("wrong password"));  
                    return;
                }
                else  // pass-Ok
                {
                    await _next(httpContext);
                }
            } else
            {
                httpContext.Response.StatusCode = 401;
                return;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
