using System.Net;

namespace Neptunee.OResponse.HttpMessages;

public record HttpMessage(HttpStatusCode StatusCode, string? Message, ExternalProps ExternalProps)
{
    
    public bool IsSuccess => StatusCode is >= HttpStatusCode.OK and <= (HttpStatusCode)299;
    public bool IsFailure => !IsSuccess;


    public static HttpMessage With(HttpStatusCode statusCode, string message, ExternalProps? externalProps = null)
        => new(statusCode, message, externalProps ?? new());

    public static HttpMessage Ok(string? message = null, ExternalProps? externalProps = null)
        => new(HttpStatusCode.OK, message, externalProps ?? new());

    public static HttpMessage BadRequest(string? message = null, ExternalProps? externalProps = null)
        => new(HttpStatusCode.BadRequest, message, externalProps ?? new());


    public HttpMessage<TValue> To<TValue>() => HttpMessage<TValue>.With(default, Message, StatusCode, ExternalProps);
    public HttpMessage<TValue> To<TValue>(TValue value) => HttpMessage<TValue>.With(value, Message, StatusCode, ExternalProps);
}