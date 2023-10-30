using Neptunee.OResponse.Results;

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

    public static OperationResponse<TResponse> If(Result result)
        => Unknown().OrIf(result);

    public OperationResponse<TResponse> OrSuccessIf(bool predicate, Action<OperationResponse<TResponse>> onFalse)
        => OnFalse(predicate, onFalse);

    public OperationResponse<TResponse> OrSuccessIf(bool predicate, Error errorOnFalse)
        => OrSuccessIf(predicate, response => response.Error(errorOnFalse));


    public OperationResponse<TResponse> OrSuccessIf(Func<bool> predicate, Action<OperationResponse<TResponse>> onFalse)
        => OnFalse(predicate(), onFalse);

    public OperationResponse<TResponse> OrSuccessIf(Func<bool> predicate, Error errorOnFalse)
        => OrSuccessIf(predicate, op => op.Error(errorOnFalse));

    public OperationResponse<TResponse> OrIf(Result result)
    {
        SetMessage(result.Message, true).SetStatusCode(result.StatusCode);
        if (result.Error is not null)
        {
            Error(result.Error);
        }
        foreach (var (key, value) in result.ExternalProps)
        {
            ExternalProp(key, value);
        }

        return this;
    }

    public OperationResponse<TResponse> OrIf(Func<Result> result)
        => OrIf(result());

    public OperationResponse<TResponse> AndSuccessIf(Func<bool> predicate, Action<OperationResponse<TResponse>> onFalse)
        => IsSuccess ? OrSuccessIf(predicate, onFalse) : this;


    public OperationResponse<TResponse> AndSuccessIf(Func<bool> predicate, Error errorOnFalse)
        => AndSuccessIf(predicate, response => response.Error(errorOnFalse));

    public OperationResponse<TResponse> AndIf(Func<Result> result)
        => IsSuccess ? OrIf(result) : this;
}