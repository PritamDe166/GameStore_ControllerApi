namespace GameStoreControllerApi.Dto.Contracts.Errors;

public sealed record ApiErrorDetail(
    string Field,
    string Code,
    string Message
);
