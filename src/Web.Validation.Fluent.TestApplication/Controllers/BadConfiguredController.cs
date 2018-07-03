namespace Byndyusoft.Dotnet.Core.Web.Validation.Fluent.TestApplication.Controllers
{
    using FluentValidation;
    using Infrastructure.Web.Validation.Fluent;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class BadConfiguredController : Controller
    {
        [HttpPost]
        [Validation(typeof(string))]
        [Route("typeIsNotIValidator")]
        public void TypeIsNotIValidator([FromBody]BadConfiguredDto data)
        {

        }

        [HttpPost]
        [Validation(typeof(BadConfiguredDtoValidator))]
        [Route("paramsWithoutFromBodyAttribute")]
        public void ParamsWithoutFromBodyAttribute(BadConfiguredDto data)
        {

        }

        [HttpPost]
        [Validation(typeof(StringValidator))]
        [Route("validatorTypeForAnotherDtoType")]
        public void ValidatorForAnotherDtoType([FromBody]BadConfiguredDto data)
        {

        }
    }

    public class BadConfiguredDto
    {
        
    }

    public class BadConfiguredDtoValidator : AbstractValidator<BadConfiguredDto>
    {
        
    }

    public class StringValidator : AbstractValidator<string>
    {

    }
}
