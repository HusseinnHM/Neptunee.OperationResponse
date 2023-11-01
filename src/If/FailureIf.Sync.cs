namespace Neptunee.OperationResponse;

public partial class Operation<TResponse>
{
    public static Operation<TResponse> FailureIf(bool predicate, Action<Operation<TResponse>> onTrue)
        => Unknown().OrFailureIf(predicate, onTrue);

    public static Operation<TResponse> FailureIf(bool predicate, Error errorOnTrue)
        => Unknown().OrFailureIf(predicate, errorOnTrue);

    public static Operation<TResponse> FailureIf(Func<bool> predicate, Action<Operation<TResponse>> onTrue)
        => Unknown().OrFailureIf(predicate, onTrue);

    public static Operation<TResponse> FailureIf(Func<bool> predicate, Error errorOnTrue) 
        => Unknown().OrFailureIf(predicate, errorOnTrue);

    public Operation<TResponse> OrFailureIf(bool predicate, Action<Operation<TResponse>> onTrue)
        => OnTrue(predicate, onTrue);

    public Operation<TResponse> OrFailureIf(bool predicate, Error errorOnTrue) 
        => OrFailureIf(predicate, response => response.Error(errorOnTrue));

    public Operation<TResponse> OrFailureIf(Func<bool> predicate, Action<Operation<TResponse>> onTrue)
        => OnTrue(predicate(), onTrue);

    public Operation<TResponse> OrFailureIf(Func<bool> predicate, Error errorOnTrue) 
        => OrFailureIf(predicate, response => response.Error(errorOnTrue));

    public Operation<TResponse> AndFailureIf(Func<bool> predicate, Action<Operation<TResponse>> onTrue)
        => IsSuccess ? OrFailureIf(predicate, onTrue) : this;


    public Operation<TResponse> AndFailureIf(Func<bool> predicate, Error errorOnTrue) 
        => AndFailureIf(predicate, response => response.Error(errorOnTrue));
}