using Neptunee.OResponse.ValidationErrors;

namespace Neptunee.OperationResponse.ValidationErrors;

public class ValidationException : Exception
{
    public ValidationException(string? code, string description, Dictionary<string, string>? externalProps = null) : base($"An ValidationException occurred")
    {
        Code = code;
        Description = description;
        ExternalProps = externalProps ?? new();
    }

    public ValidationException(ValidationError validationError, Dictionary<string, string>? externalProps = null) : this(null, validationError.Description, externalProps)
    {
    }   
    public ValidationException(SpecificValidationError validationError, Dictionary<string, string>? externalProps = null) : this(validationError.Code, validationError.Description, externalProps)
    {
    }

    public string? Code { get; }
    public string Description { get; }
    public Dictionary<string, string>? ExternalProps { get; }

    public static void ThrowIf(bool condition, ValidationException validationException)
    {
        if (condition)
        {
            throw validationException;
        }
    }
    public static void ThrowIf(bool condition, ValidationError validationError, Dictionary<string, string>? externalProps = null)
    {
        ThrowIf(condition, new ValidationException(validationError, externalProps));
    }

    public static void ThrowIf(Func<bool> condition, ValidationException validationException)
    {
        ThrowIf(condition(),validationException);
    }

    public static void ThrowIf(Func<bool> condition, ValidationError validationError, Dictionary<string, string>? externalProps = null)
    {
        ThrowIf(condition, new ValidationException(validationError, externalProps));
    }
 
}