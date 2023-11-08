using System.Net;
using Xunit.Abstractions;

namespace Neptunee.OperationResponse.Test;

public class UnitTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    public void TowFailures_SecondWillNotExecute()
    {
        var passed = false;
        var successMessage = "100% works";
        var failureMessage = "0% works";

        var operation = Operation<NoResponse>
            .SuccessIf(false, "1st")
            .AndSuccessIf(() =>
            {
                while (true)
                {
                }
            }, "2nd")
            .SetMessageOnSuccess(successMessage)
            .SetMessageOnFailure(failureMessage)
            .OnSuccess(_ =>
            {
                passed = true;
                _testOutputHelper.WriteLine("Wow, its work !");
            })
            .OnFailure(op => _testOutputHelper.Log(op));

        Assert.True(!passed);
        Assert.True(operation.IsFailure);
        Assert.True(operation.Errors!.Count == 1);
        Assert.Equal(failureMessage, operation.Message);
        Assert.Equal(HttpStatusCode.BadRequest, operation.StatusCode);
    }

    [Fact]
    public void TowFailures_SecondWillExecute()
    {
        var passed = false;
        var message = "a message";

        var operation = Operation<NoResponse>
            .SuccessIf(false, "Fist")
            .OrSuccessIf(MockFalse, op => op.SetMessage(message).Error("Second"))
            .OnSuccess(op =>
            {
                passed = true;
                _testOutputHelper.WriteLine("Wow, its work !");
            })
            .OnFailure(op => _testOutputHelper.Log(op));

        Assert.True(!passed);
        Assert.True(operation.IsFailure);
        Assert.True(operation.Errors!.Count == 2);
        Assert.Equal(message, operation.Message);
        Assert.Equal(HttpStatusCode.BadRequest, operation.StatusCode);
    }

    [Fact]
    public async Task AllSuccessIfMethods()
    {
        var flag = false;
        var stringError = string.Empty;

        var operation = await Operation<NoResponse>
            .SuccessIfAsync(MockTrueAsync, MockNothingAction)
            .OrSuccessIf(true, stringError)
            .OrSuccessIf(true, MockValidationErrors.MockError)
            .OrSuccessIf(MockTrue, MockNothingAction)
            .OrSuccessIf(MockTrue, stringError)
            .OrIf(Result.Ok())
            .OrIf(MockOkResult)
            .AndSuccessIf(MockTrue, MockNothingAction)
            .AndSuccessIf(MockTrue, stringError)
            .AndIf(MockOkResult)
            .OrSuccessIfAsync(MockTrueAsync, MockNothingAction)
            .OrSuccessIfAsync(MockTrueAsync, stringError)
            .OrIfAsync(MockOkResultAsync)
            .AndSuccessIfAsync(MockTrueAsync, MockNothingAction)
            .AndSuccessIfAsync(MockTrueAsync, stringError)
            .AndIfAsync(MockOkResultAsync)
            .SetMessageOnSuccess("I <3 .NET")
            .OnSuccess(_ =>
            {
                flag = true;
                _testOutputHelper.WriteLine("Wow!");
            })
            .OnSuccessAsync(async _ =>
            {
                _testOutputHelper.WriteLine("Hi, again.");
                await MockTrueAsync();
            });

        Assert.True(flag);
        Assert.True(operation.IsSuccess);
        Assert.Empty(operation.Errors);
        Assert.NotNull(operation.Message);
        Assert.Equal(HttpStatusCode.OK, operation.StatusCode);
    }

    [Fact]
    public async Task AllFailureIfMethods()
    {
        var flag = false;
        var stringError = string.Empty;

        var operation = await Operation<NoResponse>
            .FailureIfAsync(MockFalseAsync, MockNothingAction)
            .OrFailureIf(false, stringError)
            .OrFailureIf(false, MockValidationErrors.MockError)
            .OrFailureIf(MockFalse, MockNothingAction)
            .OrFailureIf(MockFalse, stringError)
            .AndFailureIf(MockFalse, MockNothingAction)
            .AndFailureIf(MockFalse, stringError)
            .OrFailureIfAsync(MockFalseAsync, MockNothingAction)
            .OrFailureIfAsync(MockFalseAsync, stringError)
            .AndFailureIfAsync(MockFalseAsync, MockNothingAction)
            .AndFailureIfAsync(MockFalseAsync, stringError)
            .SetMessageOnSuccess("I <3 .NET")
            .OnSuccess(_ =>
            {
                flag = true;
                _testOutputHelper.WriteLine("Wow!");
            })
            .OnSuccessAsync(async _ =>
            {
                _testOutputHelper.WriteLine("Hi, again.");
                await MockTrueAsync();
            });

        Assert.True(flag);
        Assert.True(operation.IsSuccess);
        Assert.Empty(operation.Errors);
        Assert.NotNull(operation.Message);
        Assert.Equal(HttpStatusCode.OK, operation.StatusCode);
    }

    private async Task<bool> MockTrueAsync() => await Task.FromResult(true);
    private async Task<bool> MockFalseAsync() => await Task.FromResult(false);

    private async Task<Result> MockOkResultAsync() => await Task.FromResult(Result.Ok());
    private bool MockTrue() => true;
    private bool MockFalse() => false;
    private Result MockOkResult() => Result.Ok();

    private void MockNothingAction<TResponse>(Operation<TResponse> response)
    {
    }
}