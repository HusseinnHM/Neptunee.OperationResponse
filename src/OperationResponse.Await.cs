using System.Net;

namespace Neptunee.OperationsResponse;

public static class AwaitableOperationResponse
{
    public static async Task<OperationResponse<TResponse>> SetMessageOnSuccess<TResponse>(this Task<OperationResponse<TResponse>> task, string message, bool overwrite = false)
        => (await task).SetMessageOnSuccess(message, overwrite);

    public static async Task<OperationResponse<TResponse>> SetMessageOnFailure<TResponse>(this Task<OperationResponse<TResponse>> task, string message, bool overwrite = false)
        => (await task).SetMessageOnFailure(message, overwrite);

    public static async Task<OperationResponse<TResponse>> SetStatusCode<TResponse>(this Task<OperationResponse<TResponse>> task, HttpStatusCode statusCode)
        => (await task).SetStatusCode(statusCode);

    public static async Task<OperationResponse<TResponse>> Error<TResponse>(this Task<OperationResponse<TResponse>> task, Error error)
        => (await task).Error(error);

    public static async Task<OperationResponse<TResponse>> ExternalProp<TResponse,TValue>(this Task<OperationResponse<TResponse>> task, string key, TValue value)
        => (await task).ExternalProp(key, value);

    public static async Task<OperationResponse<TResponse>> OnSuccess<TResponse>(this Task<OperationResponse<TResponse>> task, Action<OperationResponse<TResponse>> action)
        => (await task).OnSuccess(action);

    public static async Task<OperationResponse<TResponse>> OnSuccessAsync<TResponse>(this Task<OperationResponse<TResponse>> task, Func<OperationResponse<TResponse>, Task> asyncAction)
        => await (await task).OnSuccessAsync(asyncAction);

    public static async Task<OperationResponse<TResponse>> OnFailure<TResponse>(this Task<OperationResponse<TResponse>> task, Action<OperationResponse<TResponse>> action)
        => (await task).OnFailure(action);

    public static async Task<OperationResponse<TResponse>> OnFailureAsync<TResponse>(this Task<OperationResponse<TResponse>> task, Func<OperationResponse<TResponse>, Task> asyncAction)
        => await (await task).OnFailureAsync(asyncAction);

}