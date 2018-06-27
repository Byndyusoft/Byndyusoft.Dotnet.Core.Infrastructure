
namespace Web.Validation.Fluent.Tests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using Xunit;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Testing;
    using TestApplication;
    using Xunit.Abstractions;


    public class BasicTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;
        private readonly ITestOutputHelper output;

        public BasicTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            this.factory = factory;
            this.output = output;
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
            Assert.Equal(@"{""message"":null,""data"":{""first"":""'first' must not be empty."",""second"":""'second' should not be equal to '0'.""}}", data);
        }

        [Fact]
        public async Task CreateValueWithInvalidValueInDto()
        {
            HttpClient cleint = factory.CreateClient();

            var content = new StringContent(@"{first:[234], second: ""sfdf""}", Encoding.UTF8, "application/json");
            var response = await cleint.PostAsync("/api/values", content);
            string data = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(@"{""message"":""Field is missing or has invalid format"",""data"":{""first"":""Field is missing or has invalid format""}}", data);
        }

        [Fact]
        public async Task GetDataWithInvalidQueryParameter()
        {
            HttpClient cleint = factory.CreateClient();

            var response = await cleint.GetAsync("/api/values/111111111111111111111111");
            string data = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            //{""message"":null,""data"":{""id"":""The value 
            Assert.Equal(@"{""message"":null,""data"":{""id"":""The value '111111111111111111111111' is not valid.""}}", data);
        }

        [Fact]
        public async Task GetCompositeError()
        {
            HttpClient cleint = factory.CreateClient();

            var content = new StringContent(@"{id:3, value: {first:null, second: 111111111111111111111111111111111111}}", Encoding.UTF8, "application/json");
            var response = await cleint.PostAsync("/api/values/composite", content);
            string data = await response.Content.ReadAsStringAsync();

            output.WriteLine(data);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(@"{""message"":null,""data"":{""value.second"":""Field is missing or has invalid format""}}", data);
        }

        [Fact]
        public async Task GetCompositeFluentValidationError()
        {
            HttpClient cleint = factory.CreateClient();

            var content = new StringContent(@"{id:3, value: {first:null, second: 1111}}", Encoding.UTF8, "application/json");
            var response = await cleint.PostAsync("/api/values/composite", content);
            string data = await response.Content.ReadAsStringAsync();

            output.WriteLine(data);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(@"{""message"":null,""data"":{""value.first"":""'first' must not be empty.""}}", data);
        }
    }
}
