using System.Net;

namespace Neptunee.OResponse.HttpMessages;

public class HttpException : Exception
{
    public HttpException(HttpMessage httpMessage) : this(httpMessage.Message, httpMessage.StatusCode, httpMessage.ExternalProps)
    {
        
    }

    public HttpException(string? message, HttpStatusCode httpStatusCode, Dictionary<string,string> externalProps = null!) : base(message)
    {
        StatusCode = httpStatusCode;
        ExternalProps = new (externalProps ?? new());
    }

    public HttpStatusCode StatusCode { get; }
    public ExternalProps ExternalProps { get; }

    public static implicit operator int?(HttpException? http) => (int?)(http?.StatusCode);
    public static implicit operator HttpStatusCode(HttpException http) => http?.StatusCode ?? default;
    public static implicit operator string(HttpException http) => http.Message;

    public override string ToString() => Message;
    
    public static void ThrowIf(bool condition, HttpMessage httpMessage)
    {
        if (condition)
        {
            throw new HttpException(httpMessage);
        }
        
    }


}