namespace Neptunee.OResponse;

public partial class OperationResponse<TResponse>
{
    public static OperationResponse<TResponse> FailureIf(bool predicate, Action<OperationResponse<TResponse>> onTrue)
        => Unknown().OrFailureIf(predicate, onTrue);

    public static OperationResponse<TResponse> FailureIf(bool predicate, Error errorOnTrue)
        => Unknown().OrFailureIf(predicate, errorOnTrue);

    public static OperationResponse<TResponse> FailureIf(Func<bool> predicate, Action<OperationResponse<TResponse>> onTrue)
        => Unknown().OrFailureIf(predicate, onTrue);

    public static OperationResponse<TResponse> FailureIf(Func<bool> predicate, Error errorOnTrue) 
        => Unknown().OrFailureIf(predicate, errorOnTrue);

    public OperationResponse<TResponse> OrFailureIf(bool predicate, Action<OperationResponse<TResponse>> onTrue)
        => OnTrue(predicate, onTrue);

    public OperationResponse<TResponse> OrFailureIf(bool predicate, Error errorOnTrue) 
        => OrFailureIf(predicate, response => response.Error(errorOnTrue));

    public OperationResponse<TResponse> OrFailureIf(Func<bool> predicate, Action<OperationResponse<TResponse>> onTrue)
        => OnTrue(predicate(), onTrue);

    public OperationResponse<TResponse> OrFailureIf(Func<bool> predicate, Error errorOnTrue) 
        => OrFailureIf(predicate, response => response.Error(errorOnTrue));

    public OperationResponse<TResponse> AndFailureIf(Func<bool> predicate, Action<OperationResponse<TResponse>> onTrue)
        => IsSuccess ? OrFailureIf(predicate, onTrue) : this;


    public OperationResponse<TResponse> AndFailureIf(Func<bool> predicate, Error errorOnTrue) 
        => AndFailureIf(predicate, response => response.Error(errorOnTrue));
}