using System.Net;
using System.Text.Json.Serialization;
using Neptunee.OResponse.HttpMessages;
using Neptunee.OResponse.ValidationErrors;

namespace Neptunee.OResponse;

public partial class OperationResponse
{
    private readonly List<ValidationError> _validationErrors = new();
    private readonly Dictionary<string, string> _externalProps = new();
    private HttpStatusCode? _statusCode;

    protected OperationResponse()
    {
    }

    protected OperationResponse(HttpStatusCode statusCode, string? message, Dictionary<string, string> externalProps)
    {
        Message = message;
        _statusCode = statusCode;
        _externalProps = externalProps;
    }

    public static OperationResponse Unknown() => new();

    public static OperationResponse Ok(string? message = null)
        => Unknown().SetStatusCode(HttpStatusCode.OK).SetMessage(message);

    public static OperationResponse BadRequest(string? message = null)
        => Unknown().SetStatusCode(HttpStatusCode.BadRequest).SetMessage(message);

    public static OperationResponse HttpMessage(HttpMessage httpMessage)
        => new(httpMessage.StatusCode, httpMessage.Message, httpMessage.ExternalProps);


    public bool IsSuccess => StatusCode is >= HttpStatusCode.OK and <= (HttpStatusCode)299;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; private set; }

    [JsonIgnore] public bool IsFailure => !IsSuccess;
    [JsonIgnore] public HttpStatusCode StatusCode => _statusCode ?? (_validationErrors.Any() ? HttpStatusCode.BadRequest : HttpStatusCode.OK);


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyDictionary<string, string>? ExternalProps => _externalProps.Any() ? _externalProps : null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyCollection<ValidationError>? ValidationErrors => _validationErrors.Any() ? _validationErrors : null;

    public virtual OperationResponse SetMessage(string? message, bool overwrite = false)
    {
        if (overwrite || Message is null)
        {
            Message = message;
        }

        return this;
    }

    public virtual OperationResponse SetMessageOnSuccess(string message, bool overwrite = false)
    {
        return OnSuccess(op => op.SetMessage(message, overwrite));
    }

    public virtual OperationResponse SetMessageOnFailure(string message, bool overwrite = false)
    {
        return OnFailure(op => op.SetMessage(message, overwrite));
    }


    public virtual OperationResponse SetStatusCode(HttpStatusCode statusCode)
    {
        _statusCode = statusCode;
        return this;
    }


    public virtual OperationResponse ValidationError(ValidationError validationError)
    {
        _validationErrors.Add(validationError);
        return this;
    }


    internal OperationResponse AddValidationErrors(List<ValidationError> validationErrors)
    {
        _validationErrors.AddRange(validationErrors);
        return this;
    }

    public virtual OperationResponse ExternalProp<TValue>(string key, TValue value)
    {
        _externalProps.TryAdd(key, value.ToString());
        return this;
    }

    public virtual OperationResponse OnSuccess(Action<OperationResponse> action)
        => OnTrue(IsSuccess, action);


    public virtual async Task<OperationResponse> OnSuccessAsync(Func<OperationResponse, Task> task)
    {
        if (IsSuccess)
        {
            await task(this);
        }

        return this;
    }

    public virtual OperationResponse OnFailure(Action<OperationResponse> action)
        => OnTrue(IsFailure, action);

    public virtual async Task<OperationResponse> OnFailureAsync(Func<OperationResponse, Task> task)
    {
        if (IsSuccess)
        {
            await task(this);
        }

        return this;
    }
    
    protected virtual OperationResponse OnTrue(bool flag, Action<OperationResponse> onTrue)
    {
        if (flag)
        {
            onTrue(this);
        }

        return this;
    }

    protected virtual OperationResponse OnFalse(bool flag, Action<OperationResponse> onFalse)
        => OnTrue(!flag, onFalse);

    public static implicit operator OperationResponse(ValidationError validationError) => BadRequest().ValidationError(validationError);
    public static implicit operator OperationResponse(HttpMessage httpMessage) => HttpMessage(httpMessage);
}