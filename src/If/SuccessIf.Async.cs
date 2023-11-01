
namespace Neptunee.OperationResponse;

public partial class Operation<TResponse>
{
    public static async Task<Operation<TResponse>> SuccessIfAsync(Func<Task<bool>> predicate, Action<Operation<TResponse>> onFalse)
        => await Unknown().OrSuccessIfAsync(predicate, onFalse);

    public static async Task<Operation<TResponse>> SuccessIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)
        => await Unknown().OrSuccessIfAsync(predicate, errorOnFalse);
    
    public async Task<Operation<TResponse>> OrSuccessIfAsync(Func<Task<bool>> predicate, Action<Operation<TResponse>> onFalse)
        => OnFalse(await predicate(), onFalse);

    public async Task<Operation<TResponse>> OrSuccessIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)
        => await OrSuccessIfAsync(predicate, op => op.Error(errorOnFalse));

    public async Task<Operation<TResponse>> OrIfAsync(Func<Task<Result>> result)
        => OrIf(await result());

    public async Task<Operation<TResponse>> AndSuccessIfAsync(Func<Task<bool>> predicate, Action<Operation<TResponse>> onFalse)
        => IsSuccess ? await OrSuccessIfAsync(predicate, onFalse) : this;
    
    public async Task<Operation<TResponse>> AndSuccessIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)
        => await AndSuccessIfAsync(predicate, response => response.Error(errorOnFalse));

    public async Task<Operation<TResponse>> AndIfAsync(Func<Task<Result>> result)
        => IsSuccess ? await OrIfAsync(result) : this;
}