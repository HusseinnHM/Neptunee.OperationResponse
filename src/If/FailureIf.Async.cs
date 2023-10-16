using Neptunee.OResponse.HttpMessages;
using Neptunee.OResponse.ValidationErrors;

namespace Neptunee.OResponse;

public partial class OperationResponse
{
    public static async Task<OperationResponse> FailureIfAsync(Func<Task<bool>> predicate, Action<OperationResponse> onTrue)
        => await Unknown().OrFailureIfAsync(predicate, onTrue);

    public static async Task<OperationResponse> FailureIfAsync(Func<Task<bool>> predicate, ValidationError errorOnFalse)
        => await Unknown().OrFailureIfAsync(predicate, errorOnFalse);


    public async Task<OperationResponse> OrFailureIfAsync(Func<Task<bool>> predicate, Action<OperationResponse> onTrue)
        => OnTrue(await predicate(), onTrue);

    public async Task<OperationResponse> OrFailureIfAsync(Func<Task<bool>> predicate, ValidationError errorOnFalse)
        => await OrFailureIfAsync(predicate, op => op.ValidationError(errorOnFalse));


    public async Task<OperationResponse> AndFailureIfAsync(Func<Task<bool>> predicate, Action<OperationResponse> onTrue)
        => IsFailure ? await OrFailureIfAsync(predicate, onTrue) : this;


    public async Task<OperationResponse> AndFailureIfAsync(Func<Task<bool>> predicate, ValidationError errorOnFalse)
        => await AndFailureIfAsync(predicate, response => response.ValidationError(errorOnFalse));
}