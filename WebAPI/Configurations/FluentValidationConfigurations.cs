using BusinessObject.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;

namespace WebAPI.Configurations
{
    public static class FluentValidationConfigurations
    {
        public static void AddFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<BaseValidator>(); // for scan all validators, must have
            services.AddFluentValidationAutoValidation(configuration =>
            {
                // Disable the built-in .NET model (data annotations) validation.
                configuration.DisableBuiltInModelValidation = false;

                // Only validate controllers decorated with the `FluentValidationAutoValidation` attribute.
                configuration.ValidationStrategy = ValidationStrategy.All;

                // Enable validation for parameters bound from `BindingSource.Body` binding sources.
                configuration.EnableBodyBindingSourceAutomaticValidation = true;

                // Enable validation for parameters bound from `BindingSource.Form` binding sources.
                configuration.EnableFormBindingSourceAutomaticValidation = true;

                // Enable validation for parameters bound from `BindingSource.Query` binding sources.
                configuration.EnableQueryBindingSourceAutomaticValidation = true;

                // Enable validation for parameters bound from `BindingSource.DefaultPath` binding sources.
                configuration.EnablePathBindingSourceAutomaticValidation = true;

                // Enable validation for parameters bound from 'BindingSource.Custom' binding sources.
                configuration.EnableCustomBindingSourceAutomaticValidation = true;

                // Replace the default result factory with a custom implementation.
                configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
            });
        }

        public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
        {
            public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
            {
                string errorMessage = validationProblemDetails != null ? string.Join(", ", validationProblemDetails.Errors.Values.Select(t => string.Join(", ", t)))
                                                                        : "";
                return new BadRequestObjectResult(new ApiResponse<object>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = errorMessage,
                    Data = null
                });
            }
        }
    }
}
