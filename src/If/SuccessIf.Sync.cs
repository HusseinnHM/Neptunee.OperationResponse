using Neptunee.OResponse.HttpMessages;
using Neptunee.OResponse.ValidationErrors;

namespace Neptunee.OResponse;

public partial class OperationResponse
{
    public static OperationResponse SuccessIf(bool predicate, Action<OperationResponse> onFalse)
        => Unknown().OrSuccessIf(predicate, onFalse);

    public static OperationResponse SuccessIf(bool predicate, ValidationError errorOnFalse)
        => Unknown().OrSuccessIf(predicate, errorOnFalse);

    public static OperationResponse SuccessIf(Func<bool> predicate, Action<OperationResponse> onFalse)
        => Unknown().OrSuccessIf(predicate, onFalse);

    public static OperationResponse SuccessIf(Func<bool> predicate, ValidationError errorOnFalse)
        => Unknown().OrSuccessIf(predicate, errorOnFalse);

    public static OperationResponse If(HttpMessage httpMessage)
        => Unknown().OrIf(httpMessage);

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
            ExternalProp(key, value);
        }

        return this;
    }

    public OperationResponse OrIf(Func<HttpMessage> httpMessage)
        => OrIf(httpMessage());

    public OperationResponse AndSuccessIf(Func<bool> predicate, Action<OperationResponse> onFalse)
        => IsSuccess ? OrSuccessIf(predicate, onFalse) : this;


    public OperationResponse AndSuccessIf(Func<bool> predicate, ValidationError errorOnFalse)
        => AndSuccessIf(predicate, response => response.ValidationError(errorOnFalse));

    public OperationResponse AndIf(Func<HttpMessage> httpMessage)
        => IsSuccess ? OrIf(httpMessage) : this;
}