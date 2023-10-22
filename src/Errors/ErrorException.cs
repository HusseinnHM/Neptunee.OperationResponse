namespace Neptunee.OResponse;

public class ErrorException : Exception
{
    public ErrorException(string? code, string description, ExternalProps? externalProps = null) : base($"An ValidationException occurred")
    {
        Code = code;
        Description = description;
        ExternalProps = externalProps ?? new();
    }

    public ErrorException(string description, ExternalProps externalProps = null!) : this(null, description, externalProps)
    {
    }

    public ErrorException(Error error, ExternalProps externalProps = null!) : this(null, error.Description, externalProps)
    {
    }

    public ErrorException(SpecificError error, ExternalProps externalProps = null!) : this(error.Code, error.Description, externalProps)
    {
    }

    public string? Code { get; }
    public string Description { get; }
    public ExternalProps ExternalProps { get; }

    public static void ThrowIf(bool condition, ErrorException errorException)
    {
        if (condition)
        {
            throw errorException;
        }
    }

    public static void ThrowIf(bool condition, Error error, ExternalProps externalProps = null!)
    {
        ThrowIf(condition, new ErrorException(error, externalProps));
    }

    public static void ThrowIf(Func<bool> condition, ErrorException errorException)
    {
        ThrowIf(condition(), errorException);
    }

    public static void ThrowIf(Func<bool> condition, Error error, ExternalProps externalProps = null!)
    {
        ThrowIf(condition, new ErrorException(error, externalProps));
    }
}