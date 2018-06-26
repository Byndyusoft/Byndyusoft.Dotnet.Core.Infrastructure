using System;

namespace Web.Validation.Fluent
{
    using System.Linq;
    using FluentValidation;
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;


    public abstract class ValidationBaseAttribute : ActionFilterAttribute
    {
        private readonly Type bodyValidatorType;

        public ValidationBaseAttribute(Type bodyValidatorType)
        {
            if(typeof(IValidator).IsAssignableFrom(bodyValidatorType) == false)
                throw new ArgumentException("bodyValidatorType must be IValidator");

            this.bodyValidatorType = bodyValidatorType;
        }

        public ValidationBaseAttribute()
        {

        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Result = CreateErrorFromModelState(actionContext, actionContext.ModelState);
                return;
            }

            if (bodyValidatorType == null)
                return;

            IValidator bodyValidator = GetBodyValidator();
            object bodyData = GetBodyData(actionContext, bodyValidator);
            if (bodyData == null)
            {
                actionContext.Result = CreateErrorForEmptyBody(actionContext);
                return;
            }

            var validationResult = bodyValidator.Validate(bodyData);
            if (!validationResult.IsValid)
            {
                actionContext.Result = CreateErrorFromValidationResult(actionContext, validationResult);
            }
        }

        protected abstract IActionResult CreateErrorFromModelState(ActionExecutingContext actionContext, ModelStateDictionary actionContextModelState);

        protected abstract IActionResult CreateErrorFromValidationResult(ActionExecutingContext actionContext, ValidationResult validationResult);

        protected abstract IActionResult CreateErrorForEmptyBody(ActionExecutingContext actionContext);

        private IValidator GetBodyValidator()
        {
            return (IValidator)Activator.CreateInstance(bodyValidatorType);
        }

        private object GetBodyData(ActionExecutingContext actionContext, IValidator bodyValidator)
        {
            ControllerParameterDescriptor descriptor = actionContext.ActionDescriptor
                .Parameters
                .Cast<ControllerParameterDescriptor>()
                .SingleOrDefault(prm => prm.ParameterInfo.GetCustomAttributes(typeof(FromBodyAttribute), inherit: true).Any());

            if (descriptor == null)
                throw new ArgumentException("Method must have FromBodyAttribute");

            if (bodyValidator.CanValidateInstancesOfType(descriptor.ParameterType) == false)
                throw new ArgumentException($"Validator {bodyValidatorType.Name} can't validate object type {descriptor.ParameterType}");

            return actionContext.ActionArguments[descriptor.Name];
        }
    }
}
