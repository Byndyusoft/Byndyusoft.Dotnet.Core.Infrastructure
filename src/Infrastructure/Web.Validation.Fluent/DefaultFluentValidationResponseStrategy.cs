namespace Web.Validation.Fluent
{
    using System.Linq;
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class DefaultFluentValidationResponseStrategy : IFluentValidationResponseStrategy
    {
        public IActionResult FromModelState(ActionExecutingContext actionContext, ModelStateDictionary actionContextModelState)
        {
            return new BadRequestObjectResult(CreateErrorFromModelState(actionContext, actionContextModelState));
        }


        protected virtual ValidationErrorDto CreateErrorFromModelState(ActionExecutingContext actionContext, ModelStateDictionary modelStateDictionary)
        {
            var criticalModelState = modelStateDictionary.Where(x => x.Value.Errors != null && x.Value.Errors.Any())
                .Select(x =>
                        {
                            var errorMessage = x.Value.Errors.First().ErrorMessage;
                            return new
                                   {
                                       x.Key,
                                       Value = string.IsNullOrEmpty(errorMessage)
                                           ? "Field is missing or has invalid format"
                                           : errorMessage
                                   };
                        })
                .ToArray();

            return new ValidationErrorDto
                   {
                       Message = criticalModelState.SingleOrDefault(x => x.Key == "")?.Value,
                       Data = criticalModelState.Where(x => x.Key != "").ToDictionary(x => x.Key, x => x.Value)
                   };
        }

        public IActionResult FromFluentValidationResult(ActionExecutingContext actionContext, ValidationResult validationResult)
        {
            var result = new ModelStateDictionary();
            foreach (var error in validationResult.Errors)
            {
                result.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return FromModelState(actionContext, result);
        }
    }
}