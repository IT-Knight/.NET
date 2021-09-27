using Microsoft.AspNetCore.Mvc.Testing;
using PrettyPrinter;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using BasicAuth_WebApi;

namespace BasicAuth_WebApi.Tests
{
    public class UnitTest2 : IClassFixture<TestFixture<Startup>>
    {        
        public HttpClient Client { get; }

        public UnitTest2(TestFixture<Startup> fixture)
        {
            Client = fixture.Client;
        }
        
        [Theory]
        [InlineData("api/auth/CheckAuth", "admin:123")]
        public async Task Test1(string url, string credentials)
        {
            // Arrange
            Client.PrettyPrint();
            credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            // Act 
            HttpResponseMessage responseMessage = await Client.GetAsync(url);
            responseMessage.PrettyPrint();

            responseMessage.EnsureSuccessStatusCode();
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(3,3);
        }

        //[Fact]
        //public async Task Test3()
        //{
        //    //HttpClient client = MyWebFactory.CreateClient();
        //}
    }
}
