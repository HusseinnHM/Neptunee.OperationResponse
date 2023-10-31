using System.Net;

namespace Neptunee.OperationsResponse.Results;

public class ResultException : Exception
{
    public ResultException(Result result) : this(result.Message, result.StatusCode, result.ExternalProps)
    {
        
    }

    public ResultException(string? message, HttpStatusCode httpStatusCode, Dictionary<string,string> externalProps = null!) : base(message)
    {
        StatusCode = httpStatusCode;
        ExternalProps = new (externalProps ?? new());
    }

    public HttpStatusCode StatusCode { get; }
    public ExternalProps ExternalProps { get; }

    public static implicit operator int?(ResultException? result) => (int?)(result?.StatusCode);
    public static implicit operator HttpStatusCode(ResultException result) => result?.StatusCode ?? default;
    public static implicit operator string(ResultException result) => result.Message;

    public override string ToString() => Message;
    
    public static void ThrowIf(bool condition, Result result)
    {
        if (condition)
        {
            throw new ResultException(result);
        }
        
    }


}