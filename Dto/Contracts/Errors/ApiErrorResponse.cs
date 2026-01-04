namespace GameStoreControllerApi.Dto.Contracts.Errors;

public sealed record ApiErrorResponse(
    string Code,
    string Message,
    ApiErrorDetail[] Details,
    string? TraceId
);
