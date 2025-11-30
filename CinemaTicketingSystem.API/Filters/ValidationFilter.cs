#region

using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Filters;

public class ValidationFilter<T>(IServiceProvider serviceProvider) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        IValidator<T>? validator = serviceProvider.GetService<IValidator<T>>();

        if (validator is null) return await next(context);

        T? requestModel = context.Arguments.OfType<T>().FirstOrDefault();


        if (requestModel is null) return await next(context);

        ValidationResult validateResult = await validator.ValidateAsync(requestModel);

        if (!validateResult.IsValid) return Results.ValidationProblem(validateResult.ToDictionary());

        return await next(context);
    }
}