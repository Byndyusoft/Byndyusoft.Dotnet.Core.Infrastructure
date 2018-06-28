namespace Web.Validation.Fluent.Tests
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using Xunit;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Testing;
    using TestApplication;
    using Xunit.Abstractions;

    public class ValidationsTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;
        private readonly ITestOutputHelper output;

        public ValidationsTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            this.factory = factory;
            this.output = output;
        }

        [Fact]
        public async Task GetValueWithoutValidator()
        {
            HttpClient cleint = factory.CreateClient();
            string data = await cleint.GetStringAsync("/api/values");

            output.WriteLine(data);
            Assert.Equal(@"[""value1"",""value2""]", data);
        }


        [Theory]
        [InlineData(@"{first: ""Not null"", second: 3}", 
                    HttpStatusCode.OK,
                    @"Not null3")]

        [InlineData(@"", 
                    HttpStatusCode.BadRequest,
                    @"{""message"":""A non-empty request body is required."",""data"":{}}")]

        [InlineData("{ value:null}", 
                    HttpStatusCode.BadRequest, 
                    @"{""message"":null,""data"":{""first"":""'first' must not be empty."",""second"":""'second' should not be equal to '0'.""}}")]

        [InlineData(@"{first:[234], second: ""sfdf""}", 
                    HttpStatusCode.BadRequest, 
                    @"{""message"":""Field is missing or has invalid format"",""data"":{""first"":""Field is missing or has invalid format""}}")]
        public async Task CreateValueTest(string request, HttpStatusCode expectedCode, string expectedResult)
        {
            HttpClient cleint = factory.CreateClient();

            var content = new StringContent(request, Encoding.UTF8, "application/json");
            var response = await cleint.PostAsync("/api/values", content);
            string data = await response.Content.ReadAsStringAsync();

            output.WriteLine(data);
            Assert.Equal(expectedCode, response.StatusCode);
            Assert.Equal(expectedResult, data);
        }

        [Fact]
        public async Task GetDataWithInvalidQueryParameter()
        {
            HttpClient cleint = factory.CreateClient();

            var response = await cleint.GetAsync("/api/values/111111111111111111111111");
            string data = await response.Content.ReadAsStringAsync();

            output.WriteLine(data);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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
