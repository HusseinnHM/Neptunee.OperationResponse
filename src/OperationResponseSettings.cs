using System.Text.Json;
using System.Text.Json.Serialization;
using Neptunee.OResponse.Converters;

namespace Neptunee.OResponse;

public static class OperationResponseSettings
{
    private static JsonConverterFactory _operationResponseConverterFactory = new IgnoreEmptyPropsOperationResponseConverterFactory();

    public static JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
        Converters = { _operationResponseConverterFactory, new SplitErrorConverter() },
    };

    public static void ResetConverterFactory<TJsonConverterFactory>(TJsonConverterFactory converterFactory) where TJsonConverterFactory : JsonConverterFactory, new()
    {
        JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions);
        JsonSerializerOptions.Converters.Remove(_operationResponseConverterFactory);
        _operationResponseConverterFactory = converterFactory;
        JsonSerializerOptions.Converters.Add(converterFactory);
    }

    public static void ResetConverterFactory<TJsonConverter>() where TJsonConverter : JsonConverterFactory, new()
        => ResetConverterFactory<TJsonConverter>(new());

    public static void ResetErrorConverter<TJsonConverter>(TJsonConverter converter) where TJsonConverter : JsonConverter<IReadOnlyCollection<Error>>, new()
    {
        JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions);
        JsonSerializerOptions.Converters.Remove(JsonSerializerOptions.GetConverter(typeof(IReadOnlyCollection<Error>)));
        JsonSerializerOptions.Converters.Add(converter);
    }

    public static void ResetErrorConverter<TJsonConverter>() where TJsonConverter : JsonConverter<IReadOnlyCollection<Error>>, new()
        => ResetErrorConverter<TJsonConverter>(new());
}