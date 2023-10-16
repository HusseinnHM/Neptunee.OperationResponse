using Neptunee.OResponse.HttpMessages;
using Neptunee.OResponse.ValidationErrors;

namespace Neptunee.OResponse;

public partial class OperationResponse
{
    public static async Task<OperationResponse> SuccessIfAsync(Func<Task<bool>> predicate, Action<OperationResponse> onFalse)
        => await Unknown().OrSuccessIfAsync(predicate, onFalse);

    public static async Task<OperationResponse> SuccessIfAsync(Func<Task<bool>> predicate, ValidationError errorOnFalse)
        => await Unknown().OrSuccessIfAsync(predicate, errorOnFalse);
    
    public async Task<OperationResponse> OrSuccessIfAsync(Func<Task<bool>> predicate, Action<OperationResponse> onFalse)
        => OnFalse(await predicate(), onFalse);

    public async Task<OperationResponse> OrSuccessIfAsync(Func<Task<bool>> predicate, ValidationError errorOnFalse)
        => await OrSuccessIfAsync(predicate, op => op.ValidationError(errorOnFalse));

    public async Task<OperationResponse> OrIfAsync(Func<Task<HttpMessage>> httpMessage)
        => OrIf(await httpMessage());

    public async Task<OperationResponse> AndSuccessIfAsync(Func<Task<bool>> predicate, Action<OperationResponse> onFalse)
        => IsSuccess ? await OrSuccessIfAsync(predicate, onFalse) : this;
    
    public async Task<OperationResponse> AndSuccessIfAsync(Func<Task<bool>> predicate, ValidationError errorOnFalse)
        => await AndSuccessIfAsync(predicate, response => response.ValidationError(errorOnFalse));

    public async Task<OperationResponse> AndIfAsync(Func<Task<HttpMessage>> httpMessage)
        => IsSuccess ? await OrIfAsync(httpMessage) : this;
}