using GameStoreControllerApi.Dto.Contracts.Errors;
using Microsoft.AspNetCore.Diagnostics;

namespace GameStoreControllerApi.Infrastructure.ExceptionHandling;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is FluentValidation.ValidationException ve)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var details = ve.Errors
                .Select(e => new ApiErrorDetail(
                    Field: e.PropertyName,
                    Code: string.IsNullOrWhiteSpace(e.ErrorCode) ? "VALIDATION_ERROR" : e.ErrorCode,
                    Message: e.ErrorMessage
                ))
                .ToArray();

            var response = new ApiErrorResponse(
                Code: "VALIDATION_FAILED",
                Message: "One or more validation errors occurred.",
                Details: details,
                TraceId: context.TraceIdentifier
            );

            await context.Response.WriteAsJsonAsync(response);
            return true;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { error = "Unexpected error." }, cancellationToken);

        return true;
    }
}
