using Neptunee.OResponse.ValidationErrors;

namespace Neptunee.OResponse;

public partial class OperationResponse
{
    #region Sync

    public static OperationResponse FailureIf(bool predicate, Action<OperationResponse> onTrue)
        => new OperationResponse().OrFailureIf(predicate, onTrue);

    public static OperationResponse FailureIf(bool predicate, ValidationError errorOnTrue)
        => new OperationResponse().OrFailureIf(predicate, errorOnTrue);

    public static OperationResponse FailureIf(Func<bool> predicate, Action<OperationResponse> onTrue)
        => new OperationResponse().OrFailureIf(predicate, onTrue);

    public static OperationResponse FailureIf(Func<bool> predicate, ValidationError errorOnTrue) 
        => new OperationResponse().OrFailureIf(predicate, errorOnTrue);

    public OperationResponse OrFailureIf(bool predicate, Action<OperationResponse> onTrue)
        => OnTrue(predicate, onTrue);

    public OperationResponse OrFailureIf(bool predicate, ValidationError errorOnTrue) 
        => OrFailureIf(predicate, response => response.ValidationError(errorOnTrue));

    public OperationResponse OrFailureIf(Func<bool> predicate, Action<OperationResponse> onTrue)
        => OnTrue(predicate(), onTrue);

    public OperationResponse OrFailureIf(Func<bool> predicate, ValidationError errorOnTrue) 
        => OrFailureIf(predicate, response => response.ValidationError(errorOnTrue));

    public OperationResponse AndFailureIf(Func<bool> predicate, Action<OperationResponse> onTrue)
        => IsSuccess ? OrFailureIf(predicate, onTrue) : this;


    public OperationResponse AndFailureIf(Func<bool> predicate, ValidationError errorOnTrue) 
        => AndFailureIf(predicate, response => response.ValidationError(errorOnTrue));

    #endregion
}