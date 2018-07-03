using System;

namespace Web.Validation.Fluent
{
    using System.Linq;
    using FluentValidation;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;


    public class ValidationAttribute : ActionFilterAttribute
    {
        private readonly Type bodyValidatorType;
        private readonly IFluentValidationResponseStrategy defaultResponseStratgy;

        public ValidationAttribute(Type bodyValidatorType)
            : this()
        {
            this.bodyValidatorType = bodyValidatorType;
        }

        public ValidationAttribute()
        {
            defaultResponseStratgy = new DefaultFluentValidationResponseStrategy();
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var responseStrategyService = actionContext.HttpContext.RequestServices.GetService(typeof(IFluentValidationResponseStrategy));
            var responseStrategy = (IFluentValidationResponseStrategy)responseStrategyService ?? defaultResponseStratgy;
            
            if (actionContext.ModelState.IsValid == false)
            {
                actionContext.Result = responseStrategy.FromModelState(actionContext, actionContext.ModelState);
                return;
            }

            if (bodyValidatorType == null)
                return;

            IValidator bodyValidator = GetBodyValidator();
            object bodyData = GetBodyData(actionContext, bodyValidator);
            if (bodyData == null)
            {
                //Пустое тело валидирует стандартный валидатор
                return;
            }

            var validationResult = bodyValidator.Validate(bodyData);
            if (validationResult.IsValid == false)
            {
                actionContext.Result = responseStrategy.FromFluentValidationResult(actionContext, validationResult);
            }
        }

        private IValidator GetBodyValidator()
        {
            if (typeof(IValidator).IsAssignableFrom(bodyValidatorType) == false)
                throw new ArgumentException("bodyValidatorType must be IValidator");

            return (IValidator) Activator.CreateInstance(bodyValidatorType);
        }

        private object GetBodyData(ActionExecutingContext actionContext, IValidator bodyValidator)
        {
            ControllerParameterDescriptor descriptor = actionContext.ActionDescriptor
                .Parameters
                .Cast<ControllerParameterDescriptor>()
                .SingleOrDefault(prm => prm.ParameterInfo.GetCustomAttributes(typeof(FromBodyAttribute), inherit: true)
                                     .Any());

            if (descriptor == null)
                throw new ArgumentException("Method must have FromBodyAttribute");

            if (bodyValidator.CanValidateInstancesOfType(descriptor.ParameterType) == false)
                throw new ArgumentException(
                    $"Validator {bodyValidatorType.Name} can't validate object type {descriptor.ParameterType}");

            return actionContext.ActionArguments[descriptor.Name];
        }
    }
}