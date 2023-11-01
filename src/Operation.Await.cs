using System.Net;

namespace Neptunee.OperationResponse;

public static class AwaitableOperation
{
    public static async Task<Operation<TResponse>> SetMessageOnSuccess<TResponse>(this Task<Operation<TResponse>> task, string message, bool overwrite = false)
        => (await task).SetMessageOnSuccess(message, overwrite);

    public static async Task<Operation<TResponse>> SetMessageOnFailure<TResponse>(this Task<Operation<TResponse>> task, string message, bool overwrite = false)
        => (await task).SetMessageOnFailure(message, overwrite);

    public static async Task<Operation<TResponse>> SetStatusCode<TResponse>(this Task<Operation<TResponse>> task, HttpStatusCode statusCode)
        => (await task).SetStatusCode(statusCode);

    public static async Task<Operation<TResponse>> Error<TResponse>(this Task<Operation<TResponse>> task, Error error)
        => (await task).Error(error);

    public static async Task<Operation<TResponse>> ExternalProp<TResponse,TValue>(this Task<Operation<TResponse>> task, string key, TValue value)
        => (await task).ExternalProp(key, value);

    public static async Task<Operation<TResponse>> OnSuccess<TResponse>(this Task<Operation<TResponse>> task, Action<Operation<TResponse>> action)
        => (await task).OnSuccess(action);

    public static async Task<Operation<TResponse>> OnSuccessAsync<TResponse>(this Task<Operation<TResponse>> task, Func<Operation<TResponse>, Task> asyncAction)
        => await (await task).OnSuccessAsync(asyncAction);

    public static async Task<Operation<TResponse>> OnFailure<TResponse>(this Task<Operation<TResponse>> task, Action<Operation<TResponse>> action)
        => (await task).OnFailure(action);

    public static async Task<Operation<TResponse>> OnFailureAsync<TResponse>(this Task<Operation<TResponse>> task, Func<Operation<TResponse>, Task> asyncAction)
        => await (await task).OnFailureAsync(asyncAction);

}