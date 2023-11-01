namespace Neptunee.OperationResponse;

public static class AwaitableFailureIf
{
    #region Sync

    public static async Task<Operation<TResponse>> OrFailureIf<TResponse>(this Task<Operation<TResponse>> task, bool predicate, Action<Operation<TResponse>> onTrue)
        => (await task).OrFailureIf(predicate, onTrue);

    public static async Task<Operation<TResponse>> OrFailureIf<TResponse>(this Task<Operation<TResponse>> task, bool predicate, Error errorOnTrue)
        => (await task).OrFailureIf(predicate, response => response.Error(errorOnTrue));

    public static async Task<Operation<TResponse>> OrFailureIf<TResponse>(this Task<Operation<TResponse>> task, Func<bool> predicate, Action<Operation<TResponse>> onTrue)
        => (await task).OrFailureIf(predicate, onTrue);

    public static async Task<Operation<TResponse>> OrFailureIf<TResponse>(this Task<Operation<TResponse>> task, Func<bool> predicate, Error errorOnTrue)
        => (await task).OrFailureIf(predicate, response => response.Error(errorOnTrue));

    public static async Task<Operation<TResponse>> AndFailureIf<TResponse>(this Task<Operation<TResponse>> task, Func<bool> predicate, Action<Operation<TResponse>> onTrue)
        => (await task).AndFailureIf(predicate, onTrue);

    public static async Task<Operation<TResponse>> AndFailureIf<TResponse>(this Task<Operation<TResponse>> task, Func<bool> predicate, Error errorOnTrue)
        => (await task).AndFailureIf(predicate, response => response.Error(errorOnTrue));

    #endregion


    #region Async

    public static async Task<Operation<TResponse>> OrFailureIfAsync<TResponse>(this Task<Operation<TResponse>> task, Func<Task<bool>> predicate, Action<Operation<TResponse>> onTrue)
        => await (await task).OrFailureIfAsync(predicate, onTrue);

    public static async Task<Operation<TResponse>> OrFailureIfAsync<TResponse>(this Task<Operation<TResponse>> task, Func<Task<bool>> predicate, Error errorOnTrue)
        => await (await task).OrFailureIfAsync(predicate, op => op.Error(errorOnTrue));

    public static async Task<Operation<TResponse>> AndFailureIfAsync<TResponse>(this Task<Operation<TResponse>> task, Func<Task<bool>> predicate, Action<Operation<TResponse>> onTrue)
        => await (await task).AndFailureIfAsync(predicate, onTrue);

    public static async Task<Operation<TResponse>> AndFailureIfAsync<TResponse>(this Task<Operation<TResponse>> task, Func<Task<bool>> predicate, Error errorOnTrue)
        => await (await task).AndFailureIfAsync(predicate, errorOnTrue);

    #endregion
}