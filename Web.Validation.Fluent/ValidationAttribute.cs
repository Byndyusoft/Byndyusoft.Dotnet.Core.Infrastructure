using System;

namespace Web.Validation.Fluent
{
    using System.Linq;
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class ValidationAttribute : ValidationBaseAttribute
    {
        public ValidationAttribute()
        {
            
        }

        public ValidationAttribute(Type bodyValidatorType)
            : base(bodyValidatorType)
        {
            
        }

        protected override IActionResult CreateErrorFromModelState(ActionExecutingContext actionContext, ModelStateDictionary actionContextModelState)
        {
            return new BadRequestObjectResult(CreateErrorFromModelState(actionContextModelState));
        }


        private ValidationErrorDto CreateErrorFromModelState(ModelStateDictionary modelStateDictionary)
        {
            var criticalModelState = modelStateDictionary.Where(x => x.Value.Errors != null && x.Value.Errors.Any())
                .ToDictionary(
                    x => GetPropertyPathWithoutDtoName(x.Key), 
                    x => x.Value.Errors.First().ErrorMessage); //$"The {x} is missing or has invalid format"

            return new ValidationErrorDto
                   {
                       Message = criticalModelState.SingleOrDefault(x => x.Key == "").Value,
                       Data = criticalModelState.Where(x => x.Key != "").ToDictionary(x => x.Key, x => x.Value)
                   };
        }

        private static readonly string[] Suffix = { ".Int32", ".Int64" };

        private string GetPropertyPathWithoutDtoName(string keyWithDtoName)
        {
            foreach (var suffix in Suffix)
            {
                if (keyWithDtoName.EndsWith(suffix))
                    keyWithDtoName = keyWithDtoName.Remove(keyWithDtoName.Length - suffix.Length);
            }

            if (!keyWithDtoName.Contains('.'))
                return keyWithDtoName;

            return string.Join(".", keyWithDtoName.Split('.').Skip(1));
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
