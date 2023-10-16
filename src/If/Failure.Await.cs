using Neptunee.OResponse.ValidationErrors;

namespace Neptunee.OResponse;

public static class FailureIfAwaitable
{
    #region Sync

    public static async Task<OperationResponse> OrFailureIf(this Task<OperationResponse> task, bool predicate, Action<OperationResponse> onTrue)
        => (await task).OrFailureIf(predicate, onTrue);

    public static async Task<OperationResponse> OrFailureIf(this Task<OperationResponse> task, bool predicate, ValidationError errorOnTrue)
        => (await task).OrFailureIf(predicate, response => response.ValidationError(errorOnTrue));

    public static async Task<OperationResponse> OrFailureIf(this Task<OperationResponse> task, Func<bool> predicate, Action<OperationResponse> onTrue)
        => (await task).OrFailureIf(predicate, onTrue);

    public static async Task<OperationResponse> OrFailureIf(this Task<OperationResponse> task, Func<bool> predicate, ValidationError errorOnTrue)
        => (await task).OrFailureIf(predicate, response => response.ValidationError(errorOnTrue));

    public static async Task<OperationResponse> AndFailureIf(this Task<OperationResponse> task, Func<bool> predicate, Action<OperationResponse> onTrue)
        => (await task).AndFailureIf(predicate, onTrue);

    public static async Task<OperationResponse> AndFailureIf(this Task<OperationResponse> task, Func<bool> predicate, ValidationError errorOnTrue)
        => (await task).AndFailureIf(predicate, response => response.ValidationError(errorOnTrue));

    #endregion


    #region Async

    public static async Task<OperationResponse> OrFailureIfAsync(this Task<OperationResponse> task, Func<Task<bool>> predicate, Action<OperationResponse> onTrue)
        => await (await task).OrFailureIfAsync(predicate, onTrue);

    public static async Task<OperationResponse> OrFailureIfAsync(this Task<OperationResponse> task, Func<Task<bool>> predicate, ValidationError errorOnFalse)
        => await (await task).OrFailureIfAsync(predicate, op => op.ValidationError(errorOnFalse));

    public static async Task<OperationResponse> AndFailureIfAsync(this Task<OperationResponse> task, Func<Task<bool>> predicate, Action<OperationResponse> onTrue)
        => await (await task).AndFailureIfAsync(predicate, onTrue);

    public static async Task<OperationResponse> AndFailureIfAsync(this Task<OperationResponse> task, Func<Task<bool>> predicate, ValidationError errorOnFalse)
        => await (await task).AndFailureIfAsync(predicate, errorOnFalse);

    #endregion
}