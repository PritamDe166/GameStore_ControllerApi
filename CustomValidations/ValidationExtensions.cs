namespace GameStoreControllerApi.CustomValidations;

public static class ValidationExtensions
{
    public static async Task ValidateOrThrowAsync<T>(this IValidator<T> validator, T instance)
    {
        var result = await validator.ValidateAsync(instance);
        if (!result.IsValid)
        {
            throw new FluentValidation.ValidationException(result.Errors);
        }
    }
}
