using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BasicAuthWebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using PrettyPrinter;
using Xunit;
using Xunit.Abstractions;


namespace TestsTry2
{
    // needs to copy the sqlite.db in here // or define absolute path
    public class UnitTest1 : IDisposable
    {
        protected TestServer _testServer;
        private readonly ITestOutputHelper _output;

        public UnitTest1(ITestOutputHelper output)
        {
            this._output = output;

            var webBuilder = new WebHostBuilder();
            webBuilder.UseStartup<Startup>();

            _testServer = new TestServer(webBuilder);
        }

        [Fact]
        public async Task Test1()
        {
            string credentials = $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes("admin: 123"))}";
            _output.WriteLine(credentials);
            var response = await _testServer.CreateRequest("/api/auth/getusers").AddHeader("Authorization", credentials).SendAsync("GET");
            _output.WriteLine(response.Headers.ToPrettyString());

            var content = await response.Content.ReadAsStringAsync();
            _output.WriteLine(content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Theory]
        [InlineData("api/auth/CheckAuth", "admin:123")]
        [InlineData("api/auth/GetUsers", "admin:123")]
        public async Task Test2(string url, string credentials)
        {
            // Arrange

            //WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { });
            //HttpClient httpClient = webHost.CreateClient();

            HttpClient httpClient = _testServer.CreateClient();

            credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            // Act 
            //HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
            var response = await _testServer.CreateRequest(url).AddHeader("Authorization", $"Basic {credentials}").SendAsync("GET");
            _output.WriteLine(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public void Dispose()  // зачем он тут?
        {
            _testServer.Dispose();
        }
    }
}
