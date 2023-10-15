using Neptunee.OResponse.HttpMessages;
using Neptunee.OResponse.ValidationErrors;

namespace Neptunee.OResponse;

public partial class OperationResponse
{
    #region Sync

    public static OperationResponse SuccessIf(bool predicate, Action<OperationResponse> onFalse)
        => new OperationResponse().OrSuccessIf(predicate, onFalse);

    public static OperationResponse SuccessIf(bool predicate, ValidationError errorOnFalse)
        => new OperationResponse().OrSuccessIf(predicate, errorOnFalse);

    public static OperationResponse SuccessIf(Func<bool> predicate, Action<OperationResponse> onFalse)
        => new OperationResponse().OrSuccessIf(predicate, onFalse);

    public static OperationResponse SuccessIf(Func<bool> predicate, ValidationError errorOnFalse)
        => new OperationResponse().OrSuccessIf(predicate, errorOnFalse);

    public static OperationResponse If(HttpMessage httpMessage)
        => new OperationResponse().OrIf(httpMessage);

    public OperationResponse OrSuccessIf(bool predicate, Action<OperationResponse> onFalse)
        => OnFalse(predicate, onFalse);

    public OperationResponse OrSuccessIf(bool predicate, ValidationError errorOnFalse)
        => OrSuccessIf(predicate, response => response.ValidationError(errorOnFalse));


    public OperationResponse OrSuccessIf(Func<bool> predicate, Action<OperationResponse> onFalse)
        => OnFalse(predicate(), onFalse);

    public OperationResponse OrSuccessIf(Func<bool> predicate, ValidationError errorOnFalse)
        => OrSuccessIf(predicate, op => op.ValidationError(errorOnFalse));

    public OperationResponse OrIf(HttpMessage httpMessage)
    {
        SetMessage(httpMessage.Message, true).SetStatusCode(httpMessage.StatusCode);
        foreach (var (key, value) in httpMessage.ExternalProps)
        {
            _externalProps.Add(key, value);
        }

        return this;
    }


    public OperationResponse AndSuccessIf(Func<bool> predicate, Action<OperationResponse> onFalse)
        => IsSuccess ? OrSuccessIf(predicate, onFalse) : this;


    public OperationResponse AndSuccessIf(Func<bool> predicate, ValidationError errorOnFalse)
        => AndSuccessIf(predicate, response => response.ValidationError(errorOnFalse));


    public OperationResponse AndIf(Func<HttpMessage> httpMessageFunc)
    {
        if (IsSuccess)
        {
            var httpMessage = httpMessageFunc();
            SetMessage(httpMessage.Message, true);
            SetStatusCode(httpMessage.StatusCode);
            foreach (var (key, value) in httpMessage.ExternalProps)
            {
                _externalProps.Add(key, value);
            }
        }

        return this;
    }

    #endregion
}