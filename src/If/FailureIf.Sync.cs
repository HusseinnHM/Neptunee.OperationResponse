namespace Neptunee.OResponse;

public partial class OperationResponse
{
    public static OperationResponse FailureIf(bool predicate, Action<OperationResponse> onTrue)
        => Unknown().OrFailureIf(predicate, onTrue);

    public static OperationResponse FailureIf(bool predicate, Error errorOnTrue)
        => Unknown().OrFailureIf(predicate, errorOnTrue);

    public static OperationResponse FailureIf(Func<bool> predicate, Action<OperationResponse> onTrue)
        => Unknown().OrFailureIf(predicate, onTrue);

    public static OperationResponse FailureIf(Func<bool> predicate, Error errorOnTrue) 
        => Unknown().OrFailureIf(predicate, errorOnTrue);

    public OperationResponse OrFailureIf(bool predicate, Action<OperationResponse> onTrue)
        => OnTrue(predicate, onTrue);

    public OperationResponse OrFailureIf(bool predicate, Error errorOnTrue) 
        => OrFailureIf(predicate, response => response.Error(errorOnTrue));

    public OperationResponse OrFailureIf(Func<bool> predicate, Action<OperationResponse> onTrue)
        => OnTrue(predicate(), onTrue);

    public OperationResponse OrFailureIf(Func<bool> predicate, Error errorOnTrue) 
        => OrFailureIf(predicate, response => response.Error(errorOnTrue));

    public OperationResponse AndFailureIf(Func<bool> predicate, Action<OperationResponse> onTrue)
        => IsSuccess ? OrFailureIf(predicate, onTrue) : this;


    public OperationResponse AndFailureIf(Func<bool> predicate, Error errorOnTrue) 
        => AndFailureIf(predicate, response => response.Error(errorOnTrue));
}