namespace Neptunee.OResponse;

public static class AwaitableFailureIf
{
    #region Sync

    public static async Task<OperationResponse> OrFailureIf(this Task<OperationResponse> task, bool predicate, Action<OperationResponse> onTrue)
        => (await task).OrFailureIf(predicate, onTrue);

    public static async Task<OperationResponse> OrFailureIf(this Task<OperationResponse> task, bool predicate, Error errorOnTrue)
        => (await task).OrFailureIf(predicate, response => response.Error(errorOnTrue));

    public static async Task<OperationResponse> OrFailureIf(this Task<OperationResponse> task, Func<bool> predicate, Action<OperationResponse> onTrue)
        => (await task).OrFailureIf(predicate, onTrue);

    public static async Task<OperationResponse> OrFailureIf(this Task<OperationResponse> task, Func<bool> predicate, Error errorOnTrue)
        => (await task).OrFailureIf(predicate, response => response.Error(errorOnTrue));

    public static async Task<OperationResponse> AndFailureIf(this Task<OperationResponse> task, Func<bool> predicate, Action<OperationResponse> onTrue)
        => (await task).AndFailureIf(predicate, onTrue);

    public static async Task<OperationResponse> AndFailureIf(this Task<OperationResponse> task, Func<bool> predicate, Error errorOnTrue)
        => (await task).AndFailureIf(predicate, response => response.Error(errorOnTrue));

    #endregion


    #region Async

    public static async Task<OperationResponse> OrFailureIfAsync(this Task<OperationResponse> task, Func<Task<bool>> predicate, Action<OperationResponse> onTrue)
        => await (await task).OrFailureIfAsync(predicate, onTrue);

    public static async Task<OperationResponse> OrFailureIfAsync(this Task<OperationResponse> task, Func<Task<bool>> predicate, Error errorOnTrue)
        => await (await task).OrFailureIfAsync(predicate, op => op.Error(errorOnTrue));

    public static async Task<OperationResponse> AndFailureIfAsync(this Task<OperationResponse> task, Func<Task<bool>> predicate, Action<OperationResponse> onTrue)
        => await (await task).AndFailureIfAsync(predicate, onTrue);

    public static async Task<OperationResponse> AndFailureIfAsync(this Task<OperationResponse> task, Func<Task<bool>> predicate, Error errorOnTrue)
        => await (await task).AndFailureIfAsync(predicate, errorOnTrue);

    #endregion
}