namespace Neptunee.OperationResponse;

public partial class Operation<TResponse>
{
    public static async Task<Operation<TResponse>> FailureIfAsync(Func<Task<bool>> predicate, Action<Operation<TResponse>> onTrue)
        => await Unknown().OrFailureIfAsync(predicate, onTrue);

    public static async Task<Operation<TResponse>> FailureIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)
        => await Unknown().OrFailureIfAsync(predicate, errorOnFalse);


    public async Task<Operation<TResponse>> OrFailureIfAsync(Func<Task<bool>> predicate, Action<Operation<TResponse>> onTrue)
        => OnTrue(await predicate(), onTrue);

    public async Task<Operation<TResponse>> OrFailureIfAsync(Func<Task<bool>> predicate, Error errorOnTrue)
        => await OrFailureIfAsync(predicate, op => op.Error(errorOnTrue));


    public async Task<Operation<TResponse>> AndFailureIfAsync(Func<Task<bool>> predicate, Action<Operation<TResponse>> onTrue)
        => IsFailure ? await OrFailureIfAsync(predicate, onTrue) : this;


    public async Task<Operation<TResponse>> AndFailureIfAsync(Func<Task<bool>> predicate, Error errorOnTrue)
        => await AndFailureIfAsync(predicate, response => response.Error(errorOnTrue));
}