namespace Neptunee.OResponse;

public class ErrorException : Exception
{
    public ErrorException(string? code, string description, Dictionary<string, string>? externalProps = null) : base($"An ValidationException occurred")
    {
        Code = code;
        Description = description;
        ExternalProps = externalProps ?? new();
    }

    public ErrorException(string description, Dictionary<string, string>? externalProps = null) : this(null, description, externalProps)
    {
    }

    public ErrorException(Error error, Dictionary<string, string>? externalProps = null) : this(null, error.Description, externalProps)
    {
    }

    public ErrorException(SpecificError error, Dictionary<string, string>? externalProps = null) : this(error.Code, error.Description, externalProps)
    {
    }

    public string? Code { get; }
    public string Description { get; }
    public Dictionary<string, string>? ExternalProps { get; }

    public static void ThrowIf(bool condition, ErrorException errorException)
    {
        if (condition)
        {
            throw errorException;
        }
    }

    public static void ThrowIf(bool condition, Error error, Dictionary<string, string>? externalProps = null)
    {
        ThrowIf(condition, new ErrorException(error, externalProps));
    }

    public static void ThrowIf(Func<bool> condition, ErrorException errorException)
    {
        ThrowIf(condition(), errorException);
    }

    public static void ThrowIf(Func<bool> condition, Error error, Dictionary<string, string>? externalProps = null)
    {
        ThrowIf(condition, new ErrorException(error, externalProps));
    }
}