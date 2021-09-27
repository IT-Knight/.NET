using Microsoft.AspNetCore.Mvc.Testing;
using PrettyPrinter;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BasicAuth_WebApi.Tests
{
    public class UnitTest1 : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public UnitTest1(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        
        [Theory]
        [InlineData("api/auth/CheckAuth", "admin:123")]
        public async Task Test1(string url, string credentials)
        {
            // Arrange

            //WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { });
            //HttpClient httpClient = webHost.CreateClient();

            HttpClient httpClient = _factory.CreateClient();
            httpClient.PrettyPrint();
            credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            // Act 
            HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
            responseMessage.PrettyPrint();

            responseMessage.EnsureSuccessStatusCode();
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(3, 4);
        }

        //[Fact]
        //public async Task Test3()
        //{
        //    //HttpClient client = MyWebFactory.CreateClient();
        //}
    }
}
