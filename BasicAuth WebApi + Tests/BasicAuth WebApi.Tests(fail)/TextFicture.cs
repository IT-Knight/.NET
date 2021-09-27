using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BasicAuth_WebApi.Data;
using BasicAuth_WebApi.Interfaces;
using BasicAuth_WebApi.Services;
using System;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;

namespace BasicAuth_WebApi.Tests
{
    public class TestFixture<TStartup> : IDisposable where TStartup : class
    {
        private readonly TestServer _server;

        public HttpClient Client { get; }

        public TestFixture()
        {
            var builder = new WebHostBuilder().UseStartup<TStartup>();
            _server = new TestServer(builder);

            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost:5000");
        }


        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}

