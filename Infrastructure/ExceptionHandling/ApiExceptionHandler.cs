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
        LogException(exception);

        ProblemDetails problem = exception switch
        {
            ValidationException ve =>
                CreateValidationProblem(context, ve),

            NotFoundException knf =>
                CreateProblem(
                    context,
                    StatusCodes.Status404NotFound,
                    "Resource not found.",
                    knf.Message),

            UnauthorizedAccessException ua =>
                CreateProblem(
                    context,
                    StatusCodes.Status403Forbidden,
                    "Access denied.",
                    ua.Message),

            InvalidOperationException ioe =>
                CreateProblem(
                    context,
                    StatusCodes.Status409Conflict,
                    "Operation not allowed.",
                    ioe.Message),

            BusinessRuleException be =>
                CreateProblem(
                    context,
                    StatusCodes.Status406NotAcceptable,
                    "Business Rule  Violated.",
                    be.Message),

            _ =>
                CreateServerErrorProblem(context)
        };

        context.Response.StatusCode =
            problem.Status ?? StatusCodes.Status500InternalServerError;

        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }

    // ---------------- Logging ----------------

    private void LogException(Exception exception)
    {
        switch (exception)
        {
            case ValidationException:
                // Expected client error — no logging
                break;

            case KeyNotFoundException:
            case UnauthorizedAccessException:
            case InvalidOperationException:
                _logger.LogWarning(exception, exception.Message);
                break;

            default:
                _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
                break;
        }
    }

    // ---------------- ProblemDetails Builders ----------------

    private static ProblemDetails CreateProblem(
        HttpContext context,
        int statusCode,
        string title,
        string detail)
        => new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

    private static ProblemDetails CreateServerErrorProblem(HttpContext context)
        => new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Unexpected error.",
            Detail = "An unexpected error occurred.",
            Instance = context.Request.Path
        };

    private static ValidationProblemDetails CreateValidationProblem(
        HttpContext context,
        ValidationException ex)
    {
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

        // Enterprise-friendly structured error details
        var coded = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => new
                {
                    code = string.IsNullOrWhiteSpace(e.ErrorCode)
                        ? "VALIDATION_ERROR"
                        : e.ErrorCode,
                    message = e.ErrorMessage
                }).ToArray()
            );

        problem.Extensions["errorDetails"] = coded;

        return problem;
    }
}

