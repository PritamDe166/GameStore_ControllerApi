namespace GameStoreControllerApi.Infrastructure.ExceptionHandling.CustomExceptions;

public abstract class AppException(string message) : Exception(message)
{
}

public sealed class NotFoundException(string resource, Object key) : AppException($"{resource} with key '{key}' was not found" )
{
}

public sealed class BusinessRuleException(string message) : AppException(message)
{
}

public sealed class ForbiddenException(string message) : AppException(message)
{
}