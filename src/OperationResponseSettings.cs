using System.Text.Json;
using System.Text.Json.Serialization;
using Neptunee.OResponse.Converters;

namespace Neptunee.OResponse;

public static class OperationResponseSettings
{
    public static JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
        Converters = { new IgnoreEmptyPropsOperationResponseConverter(), new SplitErrorConverter() },
    };

    public static void ResetConverter<TJsonConverter>(TJsonConverter converter) where TJsonConverter : JsonConverter<OperationResponse>, new()
    {
        JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions);
        JsonSerializerOptions.Converters.Remove(JsonSerializerOptions.GetConverter(typeof(OperationResponse)));
        JsonSerializerOptions.Converters.Add(converter);

    }

    public static void ResetConverter<TJsonConverter>() where TJsonConverter : JsonConverter<OperationResponse>, new()
        => ResetConverter<TJsonConverter>(new());

    public static void ResetErrorConverter<TJsonConverter>(TJsonConverter converter) where TJsonConverter : JsonConverter<IReadOnlyCollection<Error>>, new()
    {
        JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions);
        JsonSerializerOptions.Converters.Remove(JsonSerializerOptions.GetConverter(typeof(IReadOnlyCollection<Error>)));
        JsonSerializerOptions.Converters.Add(converter);

    }

    public static void ResetErrorConverter<TJsonConverter>() where TJsonConverter : JsonConverter<IReadOnlyCollection<Error>>, new()
        => ResetErrorConverter<TJsonConverter>(new());
}