using Microsoft.AspNetCore.Diagnostics;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ValidationException = FluentValidation.ValidationException;

namespace GameStoreControllerApi.Infrastructure.ExceptionHandling;

public sealed class ApiExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ApiExceptionHandler> _logger;

    public ApiExceptionHandler(ILogger<ApiExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Don’t log ValidationException as Error (it’s expected).
        if (exception is not ValidationException)
        {
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
        }

        ProblemDetails problem = exception switch
        {
            ValidationException ve => CreateValidationProblem(context, ve),
            _ => CreateServerErrorProblem(context)
        };

        context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }

    private static ProblemDetails CreateServerErrorProblem(HttpContext context)
        => new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Unexpected error.",
            Detail = "An unexpected error occurred.",
            Instance = context.Request.Path
        };

    private static ValidationProblemDetails CreateValidationProblem(HttpContext context, ValidationException ex)
    {
        // Standard ASP.NET validation dictionary (messages)
        var errors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        var problem = new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred.",
            Instance = context.Request.Path
        };

        // Enterprise add-on: stable codes for clients (no string comparisons)
        var coded = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => new
                {
                    code = string.IsNullOrWhiteSpace(e.ErrorCode) ? "VALIDATION_ERROR" : e.ErrorCode,
                    message = e.ErrorMessage
                }).ToArray()
            );

        problem.Extensions["errorDetails"] = coded;

        return problem;
    }
}
