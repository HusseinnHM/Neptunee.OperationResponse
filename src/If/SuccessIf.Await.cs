using Neptunee.OResponse.HttpMessages;

namespace Neptunee.OResponse;

public static class AwaitableSuccessIf
{
    #region Sync

    public static async Task<OperationResponse<TResponse>> OrSuccessIf<TResponse>(this Task<OperationResponse<TResponse>> task, bool predicate, Action<OperationResponse<TResponse>> onFalse)
        => (await task).OrSuccessIf(predicate, onFalse);

    public static async Task<OperationResponse<TResponse>> OrSuccessIf<TResponse>(this Task<OperationResponse<TResponse>> task, bool predicate, Error errorOnFalse)
        => (await task).OrSuccessIf(predicate, response => response.Error(errorOnFalse));

    public static async Task<OperationResponse<TResponse>> OrSuccessIf<TResponse>(this Task<OperationResponse<TResponse>> task, Func<bool> predicate, Action<OperationResponse<TResponse>> onFalse)
        => (await task).OrSuccessIf(predicate, onFalse);

    public static async Task<OperationResponse<TResponse>> OrSuccessIf<TResponse>(this Task<OperationResponse<TResponse>> task, Func<bool> predicate, Error errorOnFalse)
        => (await task).OrSuccessIf(predicate, errorOnFalse);

    public static async Task<OperationResponse<TResponse>> OrIf<TResponse>(this Task<OperationResponse<TResponse>> task, HttpMessage httpMessage)
        => (await task).OrIf(httpMessage);

    public static async Task<OperationResponse<TResponse>> OrIf<TResponse>(this Task<OperationResponse<TResponse>> task, Func<HttpMessage> httpMessage)
        => (await task).OrIf(httpMessage);

    public static async Task<OperationResponse<TResponse>> AndSuccessIf<TResponse>(this Task<OperationResponse<TResponse>> task, Func<bool> predicate, Action<OperationResponse<TResponse>> onFalse)
        => (await task).AndSuccessIf(predicate, onFalse);

    public static async Task<OperationResponse<TResponse>> AndSuccessIf<TResponse>(this Task<OperationResponse<TResponse>> task, Func<bool> predicate, Error errorOnFalse)
        => (await task).AndSuccessIf(predicate, errorOnFalse);

    public static async Task<OperationResponse<TResponse>> AndIf<TResponse>(this Task<OperationResponse<TResponse>> task, Func<HttpMessage> httpMessage)
        => (await task).AndIf(httpMessage);

    #endregion

    #region Async

    public static async Task<OperationResponse<TResponse>> OrSuccessIfAsync<TResponse>(this Task<OperationResponse<TResponse>> task, Func<Task<bool>> predicate, Action<OperationResponse<TResponse>> onFalse)
        => await (await task).OrSuccessIfAsync(predicate, onFalse);

    public static async Task<OperationResponse<TResponse>> OrSuccessIfAsync<TResponse>(this Task<OperationResponse<TResponse>> task, Func<Task<bool>> predicate, Error errorOnFalse)
        => await (await task).OrSuccessIfAsync(predicate, errorOnFalse);

    public static async Task<OperationResponse<TResponse>> OrIfAsync<TResponse>(this Task<OperationResponse<TResponse>> task, Func<Task<HttpMessage>> httpMessage)
        => await (await task).OrIfAsync(httpMessage);

    public static async Task<OperationResponse<TResponse>> AndSuccessIfAsync<TResponse>(this Task<OperationResponse<TResponse>> task, Func<Task<bool>> predicate, Action<OperationResponse<TResponse>> onFalse)
        => await (await task).AndSuccessIfAsync(predicate, onFalse);

    public static async Task<OperationResponse<TResponse>> AndSuccessIfAsync<TResponse>(this Task<OperationResponse<TResponse>> task, Func<Task<bool>> predicate, Error errorOnFalse)
        => await (await task).AndSuccessIfAsync(predicate, errorOnFalse);

    public static async Task<OperationResponse<TResponse>> AndIfAsync<TResponse>(this Task<OperationResponse<TResponse>> task, Func<Task<HttpMessage>> httpMessage)
        => await (await task).AndIfAsync(httpMessage);

    #endregion
}