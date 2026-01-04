
namespace GameStoreControllerApi.CustomValidations;

public sealed class CreateGameValidatorServiceValidations : AbstractValidator<CreateGameDto>
{
    public CreateGameValidatorServiceValidations()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode(ValidationCodes.NameRequired)
            .MaximumLength(100)
            .WithErrorCode(ValidationCodes.NameMaxLength)
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithErrorCode(ValidationCodes.NameWhitespace)
            .WithMessage("Name cannot be empty or whitespace.");

        RuleFor(x => x.GenreId)
            .GreaterThan(0)
            .WithErrorCode(ValidationCodes.GenreIdInvalid);

        RuleFor(x => x.Price)
            .InclusiveBetween(0.99m, 99.99m)
            .WithErrorCode(ValidationCodes.PriceOutOfRange);

        RuleFor(x => x.ReleaseDate)
            .NotEqual(default(DateOnly))
            .WithErrorCode(ValidationCodes.ReleaseDateRequired)
            .WithMessage("ReleaseDate is required.")
            .Must(d => d <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithErrorCode(ValidationCodes.ReleaseDateFuture)
            .WithMessage("ReleaseDate cannot be in the future.");
    }
}
