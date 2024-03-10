using System.Net;
using Microsoft.AspNetCore.Http;


namespace Neptunee.OperationResponse;

public partial class Operation<TResponse>
{
    private readonly List<Error> _errors = new();
    private HttpStatusCode? _statusCode;

    protected Operation()
    {
    }

    public static Operation<TResponse> Unknown() => new();

    public static Operation<TResponse> Ok(string? message = null)
        => Unknown().SetStatusCode(HttpStatusCode.OK).SetMessage(message);

    public static Operation<TResponse> BadRequest(string? message = null)
        => Unknown().SetStatusCode(HttpStatusCode.BadRequest).SetMessage(message);

    public static Operation<TResponse> Result(Result result)
    {
        var op = Unknown().SetStatusCode(result.StatusCode).SetMessage(result.Message);
        if (result.Error is not null)
        {
            op.Error(result.Error);
        }

        op.ExternalProps = new(result.ExternalProps);
        return op;
    }

    public static Operation<TResponse> Result(Result<TResponse> result)
        => Result(result as Result).SetResponse(result.ValueOrDefault()!);

    public string? Message { get; private set; }
    public TResponse? Response { get; set; }
    public ExternalProps ExternalProps { get; private set; } = new();

    public bool IsSuccess => StatusCode is >= HttpStatusCode.OK and <= (HttpStatusCode)299;
    public bool IsFailure => !IsSuccess;
    public bool HasResponse => Response is not null;
    public HttpStatusCode StatusCode => _statusCode ?? (_errors.Any() ? HttpStatusCode.BadRequest : HttpStatusCode.OK);

    public IReadOnlyCollection<Error> Errors => _errors;

    public virtual Operation<TResponse> SetResponse(TResponse? response)
    {
        Response = response;
        return this;
    }

    public virtual Operation<TResponse> SetMessage(string? message, bool overwrite = false)
    {
        if (overwrite || Message is null)
        {
            Message = message;
        }

        return this;
    }

    public virtual Operation<TResponse> SetMessageOnSuccess(string message, bool overwrite = false)
    {
        return OnSuccess(op => op.SetMessage(message, overwrite));
    }

    public virtual Operation<TResponse> SetMessageOnFailure(string message, bool overwrite = false)
    {
        return OnFailure(op => op.SetMessage(message, overwrite));
    }


    public virtual Operation<TResponse> SetStatusCode(HttpStatusCode statusCode)
    {
        _statusCode = statusCode;
        return this;
    }


    public virtual Operation<TResponse> Error(Error error)
    {
        _errors.Add(error);
        return this;
    }

    public virtual Operation<TResponse> ExternalProp<TValue>(string key, TValue value)
    {
        ExternalProps.TryAdd(key, value);
        return this;
    }

    public virtual Operation<TResponse> OnSuccess(Action<Operation<TResponse>> action)
        => OnTrue(IsSuccess, action);


    public virtual async Task<Operation<TResponse>> OnSuccessAsync(Func<Operation<TResponse>, Task> task)
    {
        if (IsSuccess)
        {
            await task(this);
        }

        return this;
    }

    public virtual Operation<TResponse> OnFailure(Action<Operation<TResponse>> action)
        => OnTrue(IsFailure, action);

    public virtual async Task<Operation<TResponse>> OnFailureAsync(Func<Operation<TResponse>, Task> task)
    {
        if (IsFailure)
        {
            await task(this);
        }

        return this;
    }

    protected virtual Operation<TResponse> OnTrue(bool flag, Action<Operation<TResponse>> onTrue)
    {
        if (flag)
        {
            onTrue(this);
        }

        return this;
    }

    protected virtual Operation<TResponse> OnFalse(bool flag, Action<Operation<TResponse>> onFalse)
        => OnTrue(!flag, onFalse);

    public static implicit operator Operation<TResponse>(Error error) => BadRequest().Error(error);
    public static implicit operator Operation<TResponse>(Result result) => Result(result);
    public static implicit operator Operation<TResponse>(TResponse response) => Ok().SetResponse(response);
}