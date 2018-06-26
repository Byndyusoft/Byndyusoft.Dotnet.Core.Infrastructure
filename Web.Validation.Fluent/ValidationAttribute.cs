using System;

namespace Web.Validation.Fluent
{
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class ValidationAttribute : ValidationBaseAttribute
    {
        protected override IActionResult CreateErrorFromModelState(ActionExecutingContext actionContext, ModelStateDictionary actionContextModelState)
        {
            return new BadRequestObjectResult("Body can't be empty");
        }

        protected override IActionResult CreateErrorFromValidationResult(ActionExecutingContext actionContext, ValidationResult validationResult)
        {
            var result = new ModelStateDictionary();
            foreach (var error in validationResult.Errors)
            {
                result.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return CreateErrorFromModelState(actionContext, result);
        }

        protected override IActionResult CreateErrorForEmptyBody(ActionExecutingContext actionContext)
        { 
            return new BadRequestObjectResult("Body can't be empty");
        }
    }
}
