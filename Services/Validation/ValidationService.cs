namespace GameStoreControllerApi.Services.Validation;

public class ValidationService : IValidationService
{
    private readonly IServiceProvider _serviceProvider;
    public ValidationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ValidateAsync<T>(T model)
    {
        var validator = _serviceProvider.GetService<IValidator<T>>();

        if (validator is null)
            return; // no validator registered — skip silently

        await validator.ValidateAndThrowAsync(model);
    }
}
