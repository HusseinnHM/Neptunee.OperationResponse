using Neptunee.OResponse.HttpMessages;

namespace Neptunee.OResponse;

public partial class OperationResponse
{
    public static async Task<OperationResponse> SuccessIfAsync(Func<Task<bool>> predicate, Action<OperationResponse> onFalse)
        => await Unknown().OrSuccessIfAsync(predicate, onFalse);

    public static async Task<OperationResponse> SuccessIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)
        => await Unknown().OrSuccessIfAsync(predicate, errorOnFalse);
    
    public async Task<OperationResponse> OrSuccessIfAsync(Func<Task<bool>> predicate, Action<OperationResponse> onFalse)
        => OnFalse(await predicate(), onFalse);

    public async Task<OperationResponse> OrSuccessIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)
        => await OrSuccessIfAsync(predicate, op => op.Error(errorOnFalse));

    public async Task<OperationResponse> OrIfAsync(Func<Task<HttpMessage>> httpMessage)
        => OrIf(await httpMessage());

    public async Task<OperationResponse> AndSuccessIfAsync(Func<Task<bool>> predicate, Action<OperationResponse> onFalse)
        => IsSuccess ? await OrSuccessIfAsync(predicate, onFalse) : this;
    
    public async Task<OperationResponse> AndSuccessIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)
        => await AndSuccessIfAsync(predicate, response => response.Error(errorOnFalse));

    public async Task<OperationResponse> AndIfAsync(Func<Task<HttpMessage>> httpMessage)
        => IsSuccess ? await OrIfAsync(httpMessage) : this;
}