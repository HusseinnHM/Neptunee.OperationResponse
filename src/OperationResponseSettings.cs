using System.Text.Json;
using System.Text.Json.Serialization;
using Neptunee.OResponse.Converters;

namespace Neptunee.OResponse;

public static class OperationResponseSettings
{
    private static JsonConverterFactory _operationResponseConverterFactory = new OperationResponseConverterFactory();

    public static JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
        Converters = { _operationResponseConverterFactory, new SplitErrorConverter() ,new IgnoreEmptyExternalPropsConverter()},
    };
    
    public static void ResetConverterFactory(JsonConverterFactory converterFactory)
    {
        JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions);
        JsonSerializerOptions.Converters.Remove(_operationResponseConverterFactory);
        _operationResponseConverterFactory = converterFactory;
        JsonSerializerOptions.Converters.Add(converterFactory);
    }

    public static void ResetConverterFactory<TJsonConverter>() where TJsonConverter : JsonConverterFactory, new()
        => ResetConverterFactory(new TJsonConverter());

    public static void ResetErrorConverter(JsonConverter<IReadOnlyCollection<Error>> converter) 
    {
        JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions);
        JsonSerializerOptions.Converters.Remove(JsonSerializerOptions.GetConverter(typeof(IReadOnlyCollection<Error>)));
        JsonSerializerOptions.Converters.Add(converter);
    }

    public static void ResetErrorConverter<TJsonConverter>() where TJsonConverter : JsonConverter<IReadOnlyCollection<Error>>, new()
        => ResetErrorConverter(new TJsonConverter());   
    
    public static void ResetExternalPropsConverter(JsonConverter<ExternalProps> converter) 
    {
        JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions);
        JsonSerializerOptions.Converters.Remove(JsonSerializerOptions.GetConverter(typeof(ExternalProps)));
        JsonSerializerOptions.Converters.Add(converter);
    }

    public static void ResetExternalPropsConverter<TJsonConverter>() where TJsonConverter : JsonConverter<ExternalProps>, new()
        => ResetExternalPropsConverter(new TJsonConverter());
}