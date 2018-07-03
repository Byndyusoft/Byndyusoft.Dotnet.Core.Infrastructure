namespace Byndyusoft.Dotnet.Core.Web.Validation.Fluent.Tests
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using FluentValidation.Results;
    using Infrastructure.Web.Validation.Fluent;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using TestApplication;
    using Xunit;
    using Xunit.Abstractions;

    public class CustomApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(x => x.AddSingleton<IFluentValidationResponseStrategy, CustomValidationResponseStrategy>());
        }
    }

    public class CustomValidationResponseStrategy : IFluentValidationResponseStrategy
    {
        public IActionResult FromModelState(ActionExecutingContext actionContext, ModelStateDictionary actionContextModelState)
        {
            return new BadRequestObjectResult("custom bad request");
        }

        public IActionResult FromFluentValidationResult(ActionExecutingContext actionContext, ValidationResult validationResult)
        {
            return new BadRequestObjectResult("custom bad request");
        }
    }

    public class CustomResponseStrategyTests : IClassFixture<CustomApplicationFactory>
    {
        private readonly CustomApplicationFactory factory;
        private readonly ITestOutputHelper output;

        public CustomResponseStrategyTests(CustomApplicationFactory factory, ITestOutputHelper output)
        {
            this.factory = factory;
            this.output = output;
        }

        [Fact]
        public async Task CreateValueTest()
        {
            HttpClient cleint = factory.CreateClient();

            var content = new StringContent("", Encoding.UTF8, "application/json");
            var response = await cleint.PostAsync("/api/values", content);
            string data = await response.Content.ReadAsStringAsync();

            output.WriteLine(data);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("custom bad request", data);
        }
    }
}
