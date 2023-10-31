using System.Net;

namespace Neptunee.OperationsResponse.Results;

public record Result(HttpStatusCode StatusCode, Error? Error = null,string? Message = null, Dictionary<string,string> ExternalProps = null!)
{
    public Dictionary<string, string> ExternalProps { get; } = ExternalProps ?? new();
    public bool IsSuccess => StatusCode is >= HttpStatusCode.OK and <= (HttpStatusCode)299;
    public bool IsFailure => !IsSuccess;


    public static Result With(HttpStatusCode statusCode, Error? error = null,string? message = null, Dictionary<string,string> externalProps = null!)
        => new(statusCode, error,message, externalProps);

    public static Result Ok(string? message = null, Dictionary<string,string> externalProps = null!)
        => new(HttpStatusCode.OK, null,message, externalProps );

    public static Result BadRequest(Error? error = null,string? message = null, Dictionary<string,string> externalProps = null!)
        => new(HttpStatusCode.BadRequest, error,message, externalProps );


    public Result<TValue> To<TValue>() => Result<TValue>.With(default,StatusCode,Error,Message,ExternalProps);
    public Result<TValue> To<TValue>(TValue value) => Result<TValue>.With(value,StatusCode,Error,Message,ExternalProps);
}