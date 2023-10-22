namespace Neptunee.OResponse;

public partial class OperationResponse<TResponse>
{
    public static async Task<OperationResponse<TResponse>> FailureIfAsync(Func<Task<bool>> predicate, Action<OperationResponse<TResponse>> onTrue)
        => await Unknown().OrFailureIfAsync(predicate, onTrue);

    public static async Task<OperationResponse<TResponse>> FailureIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)
        => await Unknown().OrFailureIfAsync(predicate, errorOnFalse);


    public async Task<OperationResponse<TResponse>> OrFailureIfAsync(Func<Task<bool>> predicate, Action<OperationResponse<TResponse>> onTrue)
        => OnTrue(await predicate(), onTrue);

    public async Task<OperationResponse<TResponse>> OrFailureIfAsync(Func<Task<bool>> predicate, Error errorOnTrue)
        => await OrFailureIfAsync(predicate, op => op.Error(errorOnTrue));


    public async Task<OperationResponse<TResponse>> AndFailureIfAsync(Func<Task<bool>> predicate, Action<OperationResponse<TResponse>> onTrue)
        => IsFailure ? await OrFailureIfAsync(predicate, onTrue) : this;


    public async Task<OperationResponse<TResponse>> AndFailureIfAsync(Func<Task<bool>> predicate, Error errorOnTrue)
        => await AndFailureIfAsync(predicate, response => response.Error(errorOnTrue));
}