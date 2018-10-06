namespace Byndyusoft.Dotnet.Core.Web.Validation.Fluent.Tests
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Testing;
    using TestApplication;
    using Xunit;
    using Xunit.Abstractions;

    public class BadConfiguredValidatorTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;
        private readonly ITestOutputHelper output;

        public BadConfiguredValidatorTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            this.factory = factory;
            this.output = output;
        }

        [Theory]
        [InlineData("typeIsNotIValidator", "bodyValidatorType must be IValidator")]
        [InlineData("paramsWithoutFromBodyAttribute", "Method must have FromBodyAttribute")]
        [InlineData("validatorTypeForAnotherDtoType", "Validator StringValidator can\'t validate object type Byndyusoft.Dotnet.Core.Web.Validation.Fluent.TestApplication.Controllers.BadConfiguredDto")]
        public async Task BadConfiguredValidationTest(string route, string errorMessage)
        {
            HttpClient cleint = factory.CreateClient();
            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await cleint.PostAsync($"/api/badConfigured/{route}", content);
            var data = await response.Content.ReadAsStringAsync();

            output.WriteLine(data);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Contains($@"""exceptionMessage"":""{errorMessage}""", data);

        }
    }
}
