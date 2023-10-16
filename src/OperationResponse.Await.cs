using System.Net;

namespace Neptunee.OResponse;

public static class AwaitableOperationResponse
{
    public static async Task<OperationResponse> SetMessageOnSuccess(this Task<OperationResponse> task, string message, bool overwrite = false)
        => (await task).SetMessageOnSuccess(message, overwrite);

    public static async Task<OperationResponse> SetMessageOnFailure(this Task<OperationResponse> task, string message, bool overwrite = false)
        => (await task).SetMessageOnFailure(message, overwrite);

    public static async Task<OperationResponse> SetStatusCode(this Task<OperationResponse> task, HttpStatusCode statusCode)
        => (await task).SetStatusCode(statusCode);

    public static async Task<OperationResponse> Error(this Task<OperationResponse> task, Error error)
        => (await task).Error(error);

    public static async Task<OperationResponse> AddErrors(this Task<OperationResponse> task, List<Error> errors)
        => (await task).AddErrors(errors);

    public static async Task<OperationResponse> ExternalProp<TValue>(this Task<OperationResponse> task, string key, TValue value)
        => (await task).ExternalProp(key, value);

    public static async Task<OperationResponse> OnSuccess(this Task<OperationResponse> task, Action<OperationResponse> action)
        => (await task).OnSuccess(action);

    public static async Task<OperationResponse> OnSuccessAsync(this Task<OperationResponse> task, Func<OperationResponse, Task> asyncAction)
        => await (await task).OnSuccessAsync(asyncAction);

    public static async Task<OperationResponse> OnFailure(this Task<OperationResponse> task, Action<OperationResponse> action)
        => (await task).OnFailure(action);

    public static async Task<OperationResponse> OnFailureAsync(this Task<OperationResponse> task, Func<OperationResponse, Task> asyncAction)
        => await (await task).OnFailureAsync(asyncAction);

}