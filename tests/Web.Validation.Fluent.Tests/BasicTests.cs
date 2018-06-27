
namespace Web.Validation.Fluent.Tests
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
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

        [Fact]
        public async Task CreateValueWithValidDto()
        {
            HttpClient cleint = factory.CreateClient();

            var content = new StringContent(@"{first: ""Not null"", second: 3}", Encoding.UTF8, "application/json");
            var response = await cleint.PostAsync("/api/values", content);
            string data = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(@"Not null3", data);
        }

        [Fact]
        public async Task CreateValueWithoutDto()
        {
            HttpClient cleint = factory.CreateClient();

            var content = new StringContent("", Encoding.UTF8, "application/json");
            var response = await cleint.PostAsync("/api/values", content);
            string data = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(@"{""message"":""A non-empty request body is required."",""data"":{}}", data);
        }

        [Fact]
        public async Task CreateValueWithoutValueInDto()
        {
            HttpClient cleint = factory.CreateClient();

            var content = new StringContent("{ value:null}", Encoding.UTF8, "application/json");
            var response = await cleint.PostAsync("/api/values", content);
            string data = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(@"{""message"":null,""data"":{""First"":""'First' must not be empty."",""Second"":""'Second' should not be equal to '0'.""}}", data);
        }

        [Fact]
        public async Task CreateValueWithInvalidValueInDto()
        {
            HttpClient cleint = factory.CreateClient();

            var content = new StringContent(@"{first:[234], second: ""sfdf""}", Encoding.UTF8, "application/json");
            var response = await cleint.PostAsync("/api/values", content);
            string data = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(@"{""message"":""The input was not valid."",""data"":{""first"":""The input was not valid.""}}", data);
        }
    }
}
