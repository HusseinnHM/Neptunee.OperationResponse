using System.Text.Json;
using Xunit.Abstractions;

namespace Neptunee.OperationResponse.Test;

public static class Helper
{
    public static void Log<TResponse>(this ITestOutputHelper testOutputHelper, Operation<TResponse> op)
    {
        testOutputHelper.WriteLine("Operation :");
        testOutputHelper.WriteLine(JsonSerializer.Serialize(op, OperationSettings.JsonSerializerOptions));
    }
}
