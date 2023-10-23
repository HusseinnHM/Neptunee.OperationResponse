using Neptunee.OResponse.HttpMessages;

namespace Neptunee.OResponse;

public partial class OperationResponse<TResponse>
{
    public static OperationResponse<TResponse> SuccessIf(bool predicate, Action<OperationResponse<TResponse>> onFalse)
        => Unknown().OrSuccessIf(predicate, onFalse);

    public static OperationResponse<TResponse> SuccessIf(bool predicate, Error errorOnFalse)
        => Unknown().OrSuccessIf(predicate, errorOnFalse);

    public static OperationResponse<TResponse> SuccessIf(Func<bool> predicate, Action<OperationResponse<TResponse>> onFalse)
        => Unknown().OrSuccessIf(predicate, onFalse);

    public static OperationResponse<TResponse> SuccessIf(Func<bool> predicate, Error errorOnFalse)
        => Unknown().OrSuccessIf(predicate, errorOnFalse);

    public static OperationResponse<TResponse> If(HttpMessage httpMessage)
        => Unknown().OrIf(httpMessage);

    public OperationResponse<TResponse> OrSuccessIf(bool predicate, Action<OperationResponse<TResponse>> onFalse)
        => OnFalse(predicate, onFalse);

    public OperationResponse<TResponse> OrSuccessIf(bool predicate, Error errorOnFalse)
        => OrSuccessIf(predicate, response => response.Error(errorOnFalse));


    public OperationResponse<TResponse> OrSuccessIf(Func<bool> predicate, Action<OperationResponse<TResponse>> onFalse)
        => OnFalse(predicate(), onFalse);

    public OperationResponse<TResponse> OrSuccessIf(Func<bool> predicate, Error errorOnFalse)
        => OrSuccessIf(predicate, op => op.Error(errorOnFalse));

    public OperationResponse<TResponse> OrIf(HttpMessage httpMessage)
    {
        SetMessage(httpMessage.Message, true).SetStatusCode(httpMessage.StatusCode);
        if (httpMessage.Error is not null)
        {
            Error(httpMessage.Error);
        }
        foreach (var (key, value) in httpMessage.ExternalProps)
        {
            ExternalProp(key, value);
        }

        return this;
    }

    public OperationResponse<TResponse> OrIf(Func<HttpMessage> httpMessage)
        => OrIf(httpMessage());

    public OperationResponse<TResponse> AndSuccessIf(Func<bool> predicate, Action<OperationResponse<TResponse>> onFalse)
        => IsSuccess ? OrSuccessIf(predicate, onFalse) : this;


    public OperationResponse<TResponse> AndSuccessIf(Func<bool> predicate, Error errorOnFalse)
        => AndSuccessIf(predicate, response => response.Error(errorOnFalse));

    public OperationResponse<TResponse> AndIf(Func<HttpMessage> httpMessage)
        => IsSuccess ? OrIf(httpMessage) : this;
}