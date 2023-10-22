﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using Neptunee.OResponse.HttpMessages;

namespace Neptunee.OResponse;

public class ExternalProps : ReadOnlyDictionary<string, string>
{
    public ExternalProps() : base(new Dictionary<string, string>())
    {
    }

    public ExternalProps(IDictionary<string, string> dictionary) : base(dictionary)
    {
    }

    internal void TryAdd<TValue>(string key, TValue value)
    {
        Dictionary!.TryAdd(key, value!.ToString());
    }
}

public class OperationResponse : OperationResponse<NoResponse>
{
}

public partial class OperationResponse<TResponse>
{
    private readonly List<Error> _errors = new();
    private HttpStatusCode? _statusCode;

    protected OperationResponse()
    {
    }

    protected OperationResponse(HttpStatusCode statusCode, string? message, ExternalProps externalProps)
    {
        Message = message;
        ExternalProps = externalProps;
        _statusCode = statusCode;
    }

    public static OperationResponse<TResponse> Unknown() => new();

    public static OperationResponse<TResponse> Ok(string? message = null)
        => Unknown().SetStatusCode(HttpStatusCode.OK).SetMessage(message);

    public static OperationResponse<TResponse> BadRequest(string? message = null)
        => Unknown().SetStatusCode(HttpStatusCode.BadRequest).SetMessage(message);

    public static OperationResponse<TResponse> HttpMessage(HttpMessage httpMessage)
        => new(httpMessage.StatusCode, httpMessage.Message, httpMessage.ExternalProps);

    public string? Message { get; private set; }
    public TResponse? Response { get; set; }
    public ExternalProps ExternalProps { get; private set; } = new();

    public bool IsSuccess => StatusCode is >= HttpStatusCode.OK and <= (HttpStatusCode)299;
    public bool IsFailure => !IsSuccess;
    public bool HasResponse => Response is not null;
    public HttpStatusCode StatusCode => _statusCode ?? (_errors.Any() ? HttpStatusCode.BadRequest : HttpStatusCode.OK);

    public IReadOnlyCollection<Error> Errors => _errors;

    public virtual OperationResponse<TResponse> SetResponse(TResponse response)
    {
        Response = response;
        return this;
    }

    public virtual OperationResponse<TResponse> SetMessage(string? message, bool overwrite = false)
    {
        if (overwrite || Message is null)
        {
            Message = message;
        }

        return this;
    }

    public virtual OperationResponse<TResponse> SetMessageOnSuccess(string message, bool overwrite = false)
    {
        return OnSuccess(op => op.SetMessage(message, overwrite));
    }

    public virtual OperationResponse<TResponse> SetMessageOnFailure(string message, bool overwrite = false)
    {
        return OnFailure(op => op.SetMessage(message, overwrite));
    }


    public virtual OperationResponse<TResponse> SetStatusCode(HttpStatusCode statusCode)
    {
        _statusCode = statusCode;
        return this;
    }


    public virtual OperationResponse<TResponse> Error(Error error)
    {
        _errors.Add(error);
        return this;
    }

    public virtual OperationResponse<TResponse> ExternalProp<TValue>(string key, TValue value)
    {
        ExternalProps.TryAdd(key, value);
        return this;
    }

    public virtual OperationResponse<TResponse> OnSuccess(Action<OperationResponse<TResponse>> action)
        => OnTrue(IsSuccess, action);


    public virtual async Task<OperationResponse<TResponse>> OnSuccessAsync(Func<OperationResponse<TResponse>, Task> task)
    {
        if (IsSuccess)
        {
            await task(this);
        }

        return this;
    }

    public virtual OperationResponse<TResponse> OnFailure(Action<OperationResponse<TResponse>> action)
        => OnTrue(IsFailure, action);

    public virtual async Task<OperationResponse<TResponse>> OnFailureAsync(Func<OperationResponse<TResponse>, Task> task)
    {
        if (IsSuccess)
        {
            await task(this);
        }

        return this;
    }

    protected virtual OperationResponse<TResponse> OnTrue(bool flag, Action<OperationResponse<TResponse>> onTrue)
    {
        if (flag)
        {
            onTrue(this);
        }

        return this;
    }

    protected virtual OperationResponse<TResponse> OnFalse(bool flag, Action<OperationResponse<TResponse>> onFalse)
        => OnTrue(!flag, onFalse);

    public static implicit operator OperationResponse<TResponse>(Error error) => BadRequest().Error(error);
    public static implicit operator OperationResponse<TResponse>(HttpMessage httpMessage) => HttpMessage(httpMessage);
}