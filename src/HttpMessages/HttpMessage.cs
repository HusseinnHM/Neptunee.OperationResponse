using System.Net;

namespace Neptunee.OResponse.HttpMessages;

public record HttpMessage(HttpStatusCode StatusCode, Error? Error = null,string? Message = null, Dictionary<string,string> ExternalProps = null!)
{
    public Dictionary<string, string> ExternalProps { get; } = ExternalProps ?? new();
    public bool IsSuccess => StatusCode is >= HttpStatusCode.OK and <= (HttpStatusCode)299;
    public bool IsFailure => !IsSuccess;


    public static HttpMessage With(HttpStatusCode statusCode, Error? error = null,string? message = null, Dictionary<string,string> externalProps = null!)
        => new(statusCode, error,message, externalProps);

    public static HttpMessage Ok(string? message = null, Dictionary<string,string> externalProps = null!)
        => new(HttpStatusCode.OK, null,message, externalProps );

    public static HttpMessage BadRequest(Error? error = null,string? message = null, Dictionary<string,string> externalProps = null!)
        => new(HttpStatusCode.BadRequest, error,message, externalProps );


    public HttpMessage<TValue> To<TValue>() => HttpMessage<TValue>.With(default,StatusCode,Error,Message,ExternalProps);
    public HttpMessage<TValue> To<TValue>(TValue value) => HttpMessage<TValue>.With(value,StatusCode,Error,Message,ExternalProps);
}