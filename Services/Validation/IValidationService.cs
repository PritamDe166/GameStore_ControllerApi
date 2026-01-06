namespace GameStoreControllerApi.Services.Validation;

public interface IValidationService
{
    Task ValidateAsync<T>(T model);
}
