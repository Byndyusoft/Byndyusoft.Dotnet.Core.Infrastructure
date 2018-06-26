
namespace Web.Validation.Fluent.Tests
{
    using System.Net.Http;
    using Xunit;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Testing;
    using TestApplication;


    public class BasicTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public BasicTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Test1()
        {
            HttpClient cleint = factory.CreateClient();
            string data = await cleint.GetStringAsync("/api/values");

            Assert.Equal(@"[""value1"",""value2""]", data);
        }
    }
}
