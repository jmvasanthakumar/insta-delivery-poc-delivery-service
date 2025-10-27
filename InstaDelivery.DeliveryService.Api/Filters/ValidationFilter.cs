using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InstaDelivery.DeliveryService.Api.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _provider;

        public ValidationFilter(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var param in context.ActionArguments)
            {
                var paramType = param.Value?.GetType();
                if (paramType == null) continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(paramType);
                var validator = _provider.GetService(validatorType) as IValidator;
                if (validator == null) continue;

                var validationContext = new ValidationContext<object>(param.Value!);
                var result = await validator.ValidateAsync(validationContext);
                if (!result.IsValid)
                {
                    var errors = result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                    context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors)
                    {
                        Title = "Validation failed",
                        Status = StatusCodes.Status400BadRequest
                    });
                    return;
                }
            }

            await next();
        }
    }
}
