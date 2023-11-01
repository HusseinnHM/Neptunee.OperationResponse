

namespace Neptunee.OperationResponse;

public static class AwaitableSuccessIf
{
    #region Sync

    public static async Task<Operation<TResponse>> OrSuccessIf<TResponse>(this Task<Operation<TResponse>> task, bool predicate, Action<Operation<TResponse>> onFalse)
        => (await task).OrSuccessIf(predicate, onFalse);

    public static async Task<Operation<TResponse>> OrSuccessIf<TResponse>(this Task<Operation<TResponse>> task, bool predicate, Error errorOnFalse)
        => (await task).OrSuccessIf(predicate, response => response.Error(errorOnFalse));

    public static async Task<Operation<TResponse>> OrSuccessIf<TResponse>(this Task<Operation<TResponse>> task, Func<bool> predicate, Action<Operation<TResponse>> onFalse)
        => (await task).OrSuccessIf(predicate, onFalse);

    public static async Task<Operation<TResponse>> OrSuccessIf<TResponse>(this Task<Operation<TResponse>> task, Func<bool> predicate, Error errorOnFalse)
        => (await task).OrSuccessIf(predicate, errorOnFalse);

    public static async Task<Operation<TResponse>> OrIf<TResponse>(this Task<Operation<TResponse>> task, Result result)
        => (await task).OrIf(result);

    public static async Task<Operation<TResponse>> OrIf<TResponse>(this Task<Operation<TResponse>> task, Func<Result> result)
        => (await task).OrIf(result);

    public static async Task<Operation<TResponse>> AndSuccessIf<TResponse>(this Task<Operation<TResponse>> task, Func<bool> predicate, Action<Operation<TResponse>> onFalse)
        => (await task).AndSuccessIf(predicate, onFalse);

    public static async Task<Operation<TResponse>> AndSuccessIf<TResponse>(this Task<Operation<TResponse>> task, Func<bool> predicate, Error errorOnFalse)
        => (await task).AndSuccessIf(predicate, errorOnFalse);

    public static async Task<Operation<TResponse>> AndIf<TResponse>(this Task<Operation<TResponse>> task, Func<Result> result)
        => (await task).AndIf(result);

    #endregion

    #region Async

    public static async Task<Operation<TResponse>> OrSuccessIfAsync<TResponse>(this Task<Operation<TResponse>> task, Func<Task<bool>> predicate, Action<Operation<TResponse>> onFalse)
        => await (await task).OrSuccessIfAsync(predicate, onFalse);

    public static async Task<Operation<TResponse>> OrSuccessIfAsync<TResponse>(this Task<Operation<TResponse>> task, Func<Task<bool>> predicate, Error errorOnFalse)
        => await (await task).OrSuccessIfAsync(predicate, errorOnFalse);

    public static async Task<Operation<TResponse>> OrIfAsync<TResponse>(this Task<Operation<TResponse>> task, Func<Task<Result>> result)
        => await (await task).OrIfAsync(result);

    public static async Task<Operation<TResponse>> AndSuccessIfAsync<TResponse>(this Task<Operation<TResponse>> task, Func<Task<bool>> predicate, Action<Operation<TResponse>> onFalse)
        => await (await task).AndSuccessIfAsync(predicate, onFalse);

    public static async Task<Operation<TResponse>> AndSuccessIfAsync<TResponse>(this Task<Operation<TResponse>> task, Func<Task<bool>> predicate, Error errorOnFalse)
        => await (await task).AndSuccessIfAsync(predicate, errorOnFalse);

    public static async Task<Operation<TResponse>> AndIfAsync<TResponse>(this Task<Operation<TResponse>> task, Func<Task<Result>> result)
        => await (await task).AndIfAsync(result);

    #endregion
}