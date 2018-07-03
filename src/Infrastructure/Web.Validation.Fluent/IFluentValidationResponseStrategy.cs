namespace Byndyusoft.Dotnet.Core.Infrastructure.Web.Validation.Fluent
{
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public interface IFluentValidationResponseStrategy
    {
        IActionResult FromModelState(ActionExecutingContext actionContext, ModelStateDictionary actionContextModelState);

        IActionResult FromFluentValidationResult(ActionExecutingContext actionContext, ValidationResult validationResult);
    }
}
