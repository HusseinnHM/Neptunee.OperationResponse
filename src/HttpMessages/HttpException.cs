using System.Net;

namespace Neptunee.OResponse.HttpMessages;

public class HttpException : Exception
{
    public HttpException(HttpMessage httpMessage) : this(httpMessage.Message!, httpMessage.StatusCode, httpMessage.ExternalProps)
    {
        
    }

    public HttpException(string message, HttpStatusCode httpStatusCode, Dictionary<string, string>? externalProps = null) : base(message)
    {
        StatusCode = httpStatusCode;
        ExternalProps = externalProps ?? new();
    }

    public HttpStatusCode StatusCode { get; }
    public Dictionary<string, string> ExternalProps { get; }

    public static implicit operator int?(HttpException? http) => (int?)(http?.StatusCode);
    public static implicit operator HttpStatusCode(HttpException http) => http?.StatusCode ?? default;
    public static implicit operator string(HttpException http) => http.Message;

    public override string ToString() => Message;
    
    
    public static void ThrowIf(bool condition, HttpException httpException)
    {
        if (condition)
        {
            throw httpException;
        }
    }
    public static void ThrowIf(bool condition, HttpMessage httpMessage)
    {
        ThrowIf(condition, new HttpException(httpMessage));
    }

    public static void ThrowIf(Func<bool> condition, HttpMessage httpMessage)
    {
        ThrowIf(condition(),httpMessage);
    }

    public static void ThrowIf(Func<bool> condition, HttpException httpException)
    {
        ThrowIf(condition(), httpException);
    }
}