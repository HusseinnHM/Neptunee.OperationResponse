using System.Net;

namespace Neptunee.OResponse.Results;

public record Result<TValue> : Result
{
    private readonly TValue? _value;

    private Result(TValue? value, HttpStatusCode statusCode, Error? error = null,string? message=null, Dictionary<string,string> externalProps=null!) :
        base(statusCode, error,message, externalProps ?? new())
    {
        _value = value;
    }

    public TValue? ValueOrDefault() => _value;
    public TValue Value => _value ?? throw new NullReferenceException();
    public bool HasValue => _value is not null;

    internal static Result<TValue> With(TValue? value,  HttpStatusCode statusCode,Error? error = null,string? message = null, Dictionary<string,string> externalProps = null!)
        => new(value, statusCode, error,message, externalProps);

    public static implicit operator TValue(Result<TValue> result) => result.Value;
    public static implicit operator Result<TValue>(TValue value) => Ok().To(value);
}