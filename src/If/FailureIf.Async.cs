namespace Neptunee.OResponse;

public partial class OperationResponse
{
    public static async Task<OperationResponse> FailureIfAsync(Func<Task<bool>> predicate, Action<OperationResponse> onTrue)
        => await Unknown().OrFailureIfAsync(predicate, onTrue);

    public static async Task<OperationResponse> FailureIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)
        => await Unknown().OrFailureIfAsync(predicate, errorOnFalse);


    public async Task<OperationResponse> OrFailureIfAsync(Func<Task<bool>> predicate, Action<OperationResponse> onTrue)
        => OnTrue(await predicate(), onTrue);

    public async Task<OperationResponse> OrFailureIfAsync(Func<Task<bool>> predicate, Error errorOnTrue)
        => await OrFailureIfAsync(predicate, op => op.Error(errorOnTrue));


    public async Task<OperationResponse> AndFailureIfAsync(Func<Task<bool>> predicate, Action<OperationResponse> onTrue)
        => IsFailure ? await OrFailureIfAsync(predicate, onTrue) : this;


    public async Task<OperationResponse> AndFailureIfAsync(Func<Task<bool>> predicate, Error errorOnTrue)
        => await AndFailureIfAsync(predicate, response => response.Error(errorOnTrue));
}