using System.Text.Json;
using System.Text.Json.Serialization;

namespace Neptunee.OResponse.Converters;

public class OperationResponseConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType) return false;

        return typeToConvert.GetGenericTypeDefinition() == typeof(OperationResponse<>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var responseType = typeToConvert.GetGenericArguments()[0];
        var genericResultType = typeof(OperationResponseConverter<>).MakeGenericType(responseType);
        return Activator.CreateInstance(genericResultType) as JsonConverter;
    }
}

internal class OperationResponseConverter<TResponse> : BaseOperationResponseConverter<TResponse>
{
    public override OperationResponse<TResponse>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<OperationResponse<TResponse>>(ref reader);


    public override void Write(Utf8JsonWriter writer, OperationResponse<TResponse> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        WriteBaseOperationResponse(writer, value, options);

        JsonSerializer.Serialize(writer, value.Errors, options);

        writer.WritePropertyName(nameof(value.ExternalProps));
        JsonSerializer.Serialize(writer, value.ExternalProps, options);

        writer.WriteEndObject();
    }
}