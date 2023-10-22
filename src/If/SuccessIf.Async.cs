using Neptunee.OResponse.HttpMessages;

namespace Neptunee.OResponse;

public partial class OperationResponse<TResponse>
{
    public static async Task<OperationResponse<TResponse>> SuccessIfAsync(Func<Task<bool>> predicate, Action<OperationResponse<TResponse>> onFalse)
        => await Unknown().OrSuccessIfAsync(predicate, onFalse);

    public static async Task<OperationResponse<TResponse>> SuccessIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)
        => await Unknown().OrSuccessIfAsync(predicate, errorOnFalse);
    
    public async Task<OperationResponse<TResponse>> OrSuccessIfAsync(Func<Task<bool>> predicate, Action<OperationResponse<TResponse>> onFalse)
        => OnFalse(await predicate(), onFalse);

    public async Task<OperationResponse<TResponse>> OrSuccessIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)
        => await OrSuccessIfAsync(predicate, op => op.Error(errorOnFalse));

    public async Task<OperationResponse<TResponse>> OrIfAsync(Func<Task<HttpMessage>> httpMessage)
        => OrIf(await httpMessage());

    public async Task<OperationResponse<TResponse>> AndSuccessIfAsync(Func<Task<bool>> predicate, Action<OperationResponse<TResponse>> onFalse)
        => IsSuccess ? await OrSuccessIfAsync(predicate, onFalse) : this;
    
    public async Task<OperationResponse<TResponse>> AndSuccessIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)
        => await AndSuccessIfAsync(predicate, response => response.Error(errorOnFalse));

    public async Task<OperationResponse<TResponse>> AndIfAsync(Func<Task<HttpMessage>> httpMessage)
        => IsSuccess ? await OrIfAsync(httpMessage) : this;
}