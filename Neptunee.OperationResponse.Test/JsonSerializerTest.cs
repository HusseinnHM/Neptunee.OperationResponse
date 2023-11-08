using Neptunee.OperationResponse.Converters;
using Xunit.Abstractions;

namespace Neptunee.OperationResponse.Test;

public class JsonSerializerTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public JsonSerializerTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    public void Default_WithResponse()
    {
        Operation<MockResponse>
            .Ok()
            .SetMessageOnSuccess("Everything under control")
            .SetResponse(new MockResponse(Guid.NewGuid(), DateTime.Now))
            .OnSuccess(op => _testOutputHelper.Log(op));

        Assert.Equal(3, OperationSettings.JsonSerializerOptions.Converters.Count);
    }

    [Fact]
    public void Default_WithNoResponse()
    {
        Operation<NoResponse>
            .Ok()
            .SetMessageOnSuccess("Everything under control")
            .OnSuccess(op => _testOutputHelper.Log(op));

        Assert.Equal(3, OperationSettings.JsonSerializerOptions.Converters.Count);
    }

    [Fact]
    public void SplitErrorConverter_WithoutErrors()
    {
        OperationSettings.ResetErrorConverter<SplitErrorConverter>();
        Operation<NoResponse>
            .Ok()
            .SetMessageOnSuccess("Everything under control")
            .OnSuccess(op => _testOutputHelper.Log(op));

        Assert.Equal(3, OperationSettings.JsonSerializerOptions.Converters.Count);
    }

    [Fact]
    public void SplitErrorConverter_WithErrors()
    {
        OperationSettings.ResetErrorConverter<SplitErrorConverter>();

        Operation<NoResponse>
            .SuccessIf(false, "How can set the error 1")
            .OrFailureIf(true, new Error("How can set the error 2"))
            .OrFailureIf(true, new SpecificError("SomeProp", "How can set the error 3"))
            .SetMessageOnFailure("One or more validation errors occur")
            .OnFailure(op => _testOutputHelper.Log(op));

        Assert.Equal(3, OperationSettings.JsonSerializerOptions.Converters.Count);
    }

    [Fact]
    public void CombineErrorConverter_WithoutErrors()
    {
        OperationSettings.ResetErrorConverter<CombineErrorConverter>();

        Operation<NoResponse>
            .Ok()
            .OnSuccess(op => _testOutputHelper.Log(op));

        Assert.Equal(3, OperationSettings.JsonSerializerOptions.Converters.Count);
    }

    [Fact]
    public void CombineErrorConverter_WithErrors()
    {
        OperationSettings.ResetErrorConverter<CombineErrorConverter>();

        Operation<NoResponse>
            .SuccessIf(false, "How can set the error 1")
            .OrFailureIf(true, new Error("How can set the error 2"))
            .OrFailureIf(true, new SpecificError("SomeProp", "How can set the error 3"))
            .SetMessageOnFailure("One or more validation errors occur")
            .OnFailure(op => _testOutputHelper.Log(op));

        Assert.Equal(3, OperationSettings.JsonSerializerOptions.Converters.Count);
    }

    [Fact]
    public void IgnoreEmptySplitErrorConverter_WithoutErrors()
    {
        OperationSettings.ResetErrorConverter<IgnoreEmptySplitErrorConverter>();

        Operation<NoResponse>
            .Ok()
            .OnSuccess(op => _testOutputHelper.Log(op));

        Assert.Equal(3, OperationSettings.JsonSerializerOptions.Converters.Count);
    }

    [Fact]
    public void IgnoreEmptySplitErrorConverter_WithErrors()
    {
        OperationSettings.ResetErrorConverter<IgnoreEmptySplitErrorConverter>();

        Operation<NoResponse>
            .SuccessIf(false, "How can set the error 1")
            .OrFailureIf(true, new Error("How can set the error 2"))
            .OrFailureIf(true, new SpecificError("SomeProp", "How can set the error 3"))
            .SetMessageOnFailure("One or more validation errors occur")
            .OnFailure(op => _testOutputHelper.Log(op));

        Assert.Equal(3, OperationSettings.JsonSerializerOptions.Converters.Count);

        Assert.Equal(3, OperationSettings.JsonSerializerOptions.Converters.Count);
    }

    [Fact]
    public void IgnoreEmptyCombineErrorConverter_WithoutErrors()
    {
        OperationSettings.ResetErrorConverter<IgnoreEmptyCombineErrorConverter>();

        Operation<NoResponse>
            .Ok()
            .OnSuccess(op => _testOutputHelper.Log(op));

        Assert.Equal(3, OperationSettings.JsonSerializerOptions.Converters.Count);
    }


    [Fact]
    public void IgnoreEmptyCombineErrorConverter_WithErrors()
    {
        OperationSettings.ResetErrorConverter<IgnoreEmptyCombineErrorConverter>();

        Operation<NoResponse>
            .SuccessIf(false, "How can set the error 1")
            .OrFailureIf(true, new Error("How can set the error 2"))
            .OrFailureIf(true, new SpecificError("SomeProp", "How can set the error 3"))
            .OnFailure(op => _testOutputHelper.Log(op));

        Assert.Equal(3, OperationSettings.JsonSerializerOptions.Converters.Count);
    }


    [Fact]
    public void ExternalPropsConverter_WithoutValues()
    {
        OperationSettings.ResetExternalPropsConverter<ExternalPropsConverter>();

        Operation<NoResponse>
            .Ok()
            .OnSuccess(op => _testOutputHelper.Log(op));

        Assert.Equal(3, OperationSettings.JsonSerializerOptions.Converters.Count);
    }

    [Fact]
    public void ExternalPropsConverter_WithValues()
    {
        OperationSettings.ResetExternalPropsConverter<ExternalPropsConverter>();

        Operation<NoResponse>
            .Ok()
            .ExternalProp("the key", "the value")
            .ExternalProp("the key 2", "the value2")
            .OnSuccess(op => _testOutputHelper.Log(op));

        Assert.Equal(3, OperationSettings.JsonSerializerOptions.Converters.Count);
    }

    [Fact]
    public void IgnoreEmptyExternalPropsConverter_WithoutValues()
    {
        OperationSettings.ResetExternalPropsConverter<ExternalPropsConverter>();

        Operation<NoResponse>
            .Ok()
            .OnSuccess(op => _testOutputHelper.Log(op));

        Assert.Equal(3, OperationSettings.JsonSerializerOptions.Converters.Count);
    }

    [Fact]
    public void IgnoreEmptyExternalPropsConverter_WithValues()
    {
        OperationSettings.ResetExternalPropsConverter<IgnoreEmptyExternalPropsConverter>();

        Operation<NoResponse>
            .Ok()
            .ExternalProp("the key", "the value")
            .ExternalProp("the key 2", "the value2")
            .OnSuccess(op => _testOutputHelper.Log(op));

        Assert.Equal(3, OperationSettings.JsonSerializerOptions.Converters.Count);
    }
}