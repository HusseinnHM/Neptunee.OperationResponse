using Neptunee.OResponse.HttpMessages;

namespace Neptunee.OResponse;

public static class AwaitableSuccessIf
{
    #region Sync

    public static async Task<OperationResponse> OrSuccessIf(this Task<OperationResponse> task, bool predicate, Action<OperationResponse> onFalse)
        => (await task).OrSuccessIf(predicate, onFalse);

    public static async Task<OperationResponse> OrSuccessIf(this Task<OperationResponse> task, bool predicate, Error errorOnFalse)
        => (await task).OrSuccessIf(predicate, response => response.Error(errorOnFalse));

    public static async Task<OperationResponse> OrSuccessIf(this Task<OperationResponse> task, Func<bool> predicate, Action<OperationResponse> onFalse)
        => (await task).OrSuccessIf(predicate, onFalse);

    public static async Task<OperationResponse> OrSuccessIf(this Task<OperationResponse> task, Func<bool> predicate, Error errorOnFalse)
        => (await task).OrSuccessIf(predicate, errorOnFalse);

    public static async Task<OperationResponse> OrIf(this Task<OperationResponse> task, HttpMessage httpMessage)
        => (await task).OrIf(httpMessage);

    public static async Task<OperationResponse> OrIf(this Task<OperationResponse> task, Func<HttpMessage> httpMessage)
        => (await task).OrIf(httpMessage);

    public static async Task<OperationResponse> AndSuccessIf(this Task<OperationResponse> task, Func<bool> predicate, Action<OperationResponse> onFalse)
        => (await task).AndSuccessIf(predicate, onFalse);

    public static async Task<OperationResponse> AndSuccessIf(this Task<OperationResponse> task, Func<bool> predicate, Error errorOnFalse)
        => (await task).AndSuccessIf(predicate, errorOnFalse);

    public static async Task<OperationResponse> AndIf(this Task<OperationResponse> task, Func<HttpMessage> httpMessage)
        => (await task).AndIf(httpMessage);

    #endregion

    #region Async

    public static async Task<OperationResponse> OrSuccessIfAsync(this Task<OperationResponse> task, Func<Task<bool>> predicate, Action<OperationResponse> onFalse)
        => await (await task).OrSuccessIfAsync(predicate, onFalse);

    public static async Task<OperationResponse> OrSuccessIfAsync(this Task<OperationResponse> task, Func<Task<bool>> predicate, Error errorOnFalse)
        => await (await task).OrSuccessIfAsync(predicate, errorOnFalse);

    public static async Task<OperationResponse> OrIfAsync(this Task<OperationResponse> task, Func<Task<HttpMessage>> httpMessage)
        => await (await task).OrIfAsync(httpMessage);

    public static async Task<OperationResponse> AndSuccessIfAsync(this Task<OperationResponse> task, Func<Task<bool>> predicate, Action<OperationResponse> onFalse)
        => await (await task).AndSuccessIfAsync(predicate, onFalse);

    public static async Task<OperationResponse> AndSuccessIfAsync(this Task<OperationResponse> task, Func<Task<bool>> predicate, Error errorOnFalse)
        => await (await task).AndSuccessIfAsync(predicate, errorOnFalse);

    public static async Task<OperationResponse> AndIfAsync(this Task<OperationResponse> task, Func<Task<HttpMessage>> httpMessage)
        => await (await task).AndIfAsync(httpMessage);

    #endregion
}