using System.Net;

namespace Neptunee.OResponse.HttpMessages;

public record HttpMessage<TValue> : HttpMessage
{
    private readonly TValue? _value;

    private HttpMessage(TValue? value, HttpStatusCode statusCode, Error? error = null,string? message=null, Dictionary<string,string> externalProps=null!) :
        base(statusCode, error,message, externalProps ?? new())
    {
        _value = value;
    }

    public TValue? ValueOrDefault() => _value;
    public TValue Value => _value ?? throw new NullReferenceException();
    public bool HasValue => _value is not null;

    internal static HttpMessage<TValue> With(TValue? value,  HttpStatusCode statusCode,Error? error = null,string? message = null, Dictionary<string,string> externalProps = null!)
        => new(value, statusCode, error,message, externalProps);

    public static implicit operator TValue(HttpMessage<TValue> httpMessage) => httpMessage.Value;
    public static implicit operator HttpMessage<TValue>(TValue value) => Ok().To(value);
}