

namespace Neptunee.OperationResponse;

public partial class Operation<TResponse>
{
    public static Operation<TResponse> SuccessIf(bool predicate, Action<Operation<TResponse>> onFalse)
        => Unknown().OrSuccessIf(predicate, onFalse);

    public static Operation<TResponse> SuccessIf(bool predicate, Error errorOnFalse)
        => Unknown().OrSuccessIf(predicate, errorOnFalse);

    public static Operation<TResponse> SuccessIf(Func<bool> predicate, Action<Operation<TResponse>> onFalse)
        => Unknown().OrSuccessIf(predicate, onFalse);

    public static Operation<TResponse> SuccessIf(Func<bool> predicate, Error errorOnFalse)
        => Unknown().OrSuccessIf(predicate, errorOnFalse);

    public static Operation<TResponse> If(Result result)
        => Unknown().OrIf(result);

    public Operation<TResponse> OrSuccessIf(bool predicate, Action<Operation<TResponse>> onFalse)
        => OnFalse(predicate, onFalse);

    public Operation<TResponse> OrSuccessIf(bool predicate, Error errorOnFalse)
        => OrSuccessIf(predicate, response => response.Error(errorOnFalse));


    public Operation<TResponse> OrSuccessIf(Func<bool> predicate, Action<Operation<TResponse>> onFalse)
        => OnFalse(predicate(), onFalse);

    public Operation<TResponse> OrSuccessIf(Func<bool> predicate, Error errorOnFalse)
        => OrSuccessIf(predicate, op => op.Error(errorOnFalse));

    public Operation<TResponse> OrIf(Result result)
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

    public Operation<TResponse> OrIf(Func<Result> result)
        => OrIf(result());

    public Operation<TResponse> AndSuccessIf(Func<bool> predicate, Action<Operation<TResponse>> onFalse)
        => IsSuccess ? OrSuccessIf(predicate, onFalse) : this;


    public Operation<TResponse> AndSuccessIf(Func<bool> predicate, Error errorOnFalse)
        => AndSuccessIf(predicate, response => response.Error(errorOnFalse));

    public Operation<TResponse> AndIf(Func<Result> result)
        => IsSuccess ? OrIf(result) : this;
}