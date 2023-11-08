using System.Net;
using Xunit.Abstractions;

namespace Neptunee.OperationResponse.Test;

public class RealSamplesTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public RealSamplesTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }



    [Fact]
    public void Success_WithNoResponse()
    {
        var request = new
        {
            Age = 20,
            Amount = 500,
        };
        var flag = false;

        var operation = Operation<NoResponse>
            .SuccessIf(request.Age >= 18, MockValidationErrors.Errors.Age)
            .OrFailureIf(request.Amount >= 1000, MockValidationErrors.Errors.AmountOverMax)
            .OrFailureIf(request.Amount <= 100, MockValidationErrors.Errors.AmountUnderMin)
            .SetMessageOnFailure("One or more validation errors occur")
            .OnSuccess(_ =>
            {
                flag = true;
            })
            .OnSuccess(op => _testOutputHelper.Log(op))
            .OnFailure(op => _testOutputHelper.Log(op));

        Assert.True(flag);
        Assert.True(operation.IsSuccess);
        Assert.Empty(operation.Errors);
        Assert.Null(operation.Message);
        Assert.Equal(HttpStatusCode.OK, operation.StatusCode);
    }

    [Fact]
    public void Success_WithResponse()
    {
        var request = new
        {
            Age = 20,
            Amount = 500,
        };
        var flag = false;

        var operation = Operation<MockResponse>
            .SuccessIf(request.Age >= 18, MockValidationErrors.Errors.Age)
            .OrFailureIf(request.Amount >= 1000, MockValidationErrors.Errors.AmountOverMax)
            .OrFailureIf(request.Amount <= 100, MockValidationErrors.Errors.AmountUnderMin)
            .SetMessageOnFailure("One or more validation errors occur")
            .OnSuccess(op =>
            {
                flag = true;
                op.SetResponse(new MockResponse(Guid.NewGuid(), DateTime.Now));
            })
            .OnSuccess(op => _testOutputHelper.Log(op))
            .OnFailure(op => _testOutputHelper.Log(op));

        Assert.True(flag);
        Assert.True(operation.IsSuccess);
        Assert.Empty(operation.Errors);
        Assert.Null(operation.Message);
        Assert.Equal(HttpStatusCode.OK, operation.StatusCode);
    }

    [Fact]
    public void Fail_WithErrors()
    {
        var request = new
        {
            Age = 6,
            Amount = 1_000_000,
        };
        var flag = false;

        var operation = Operation<NoResponse>
            .SuccessIf(request.Age >= 18, MockValidationErrors.Errors.Age)
            .OrFailureIf(request.Amount >= 1000, MockValidationErrors.Errors.AmountOverMax)
            .OrFailureIf(request.Amount <= 100, MockValidationErrors.Errors.AmountUnderMin)
            .SetMessageOnFailure("One or more validation errors occur")
            .OnSuccess(_ =>
            {
                flag = true;
                _testOutputHelper.WriteLine("100%");
            })
            .OnFailure(op => _testOutputHelper.Log(op));

        Assert.False(flag);
        Assert.False(operation.IsSuccess);
        Assert.NotEmpty(operation.Errors);
        Assert.NotNull(operation.Message);
        Assert.Equal(HttpStatusCode.BadRequest, operation.StatusCode);
    }

    [Fact]
    public void Fail_WithSpecificError()
    {
        var request = new
        {
            Age = 6,
            Amount = 1_000_000,
        };
        var flag = false;

        var operation = Operation<NoResponse>
            .SuccessIf(request.Age >= 18, MockValidationErrors.SpecificErrors.Age)
            .OrFailureIf(request.Amount >= 1000, MockValidationErrors.SpecificErrors.AmountOverMax)
            .OrFailureIf(request.Amount <= 100, MockValidationErrors.SpecificErrors.AmountUnderMin)
            .SetMessageOnFailure("One or more validation errors occur")
            .OnSuccess(_ =>
            {
                flag = true;
                _testOutputHelper.WriteLine("100%");
            })
            .OnFailure(op => _testOutputHelper.Log(op));

        Assert.False(flag);
        Assert.False(operation.IsSuccess);
        Assert.NotEmpty(operation.Errors);
        Assert.NotNull(operation.Message);
        Assert.Equal(HttpStatusCode.BadRequest, operation.StatusCode);
    }
}