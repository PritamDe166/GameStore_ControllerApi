namespace GameStoreControllerApi.CustomValidations;

public sealed class NotWhiteSpaceAttribute : ValidationAttribute
{
    public NotWhiteSpaceAttribute(): base("The {0} field cannot be empty or consist only of white-space characters.")
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Let [Required] handle null checks
        }
        
        return value is string str && !string.IsNullOrWhiteSpace(str);
    }
}

public sealed class ValidReleaseDateAttribute : ValidationAttribute
{
    public ValidReleaseDateAttribute() : base("The {0} field must be a valid date not in the future.")
    {
    }
    public override bool IsValid(object? value)
    {
        if (value is not DateOnly date)
            return false;

        var today = DateOnly.FromDateTime(DateTime.Today);

        return date <= today;
    }
}