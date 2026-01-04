namespace GameStoreControllerApi.CustomValidations;

public static class ValidationCodes
{
    public const string NameRequired = "GAME_NAME_REQUIRED";
    public const string NameWhitespace = "GAME_NAME_WHITESPACE";
    public const string NameMaxLength = "GAME_NAME_MAX_LENGTH";
    public const string GenreIdInvalid = "GAME_GENREID_INVALID";
    public const string PriceOutOfRange = "GAME_PRICE_OUT_OF_RANGE";
    public const string ReleaseDateRequired = "GAME_RELEASEDATE_REQUIRED";
    public const string ReleaseDateFuture = "GAME_RELEASEDATE_FUTURE";
}
