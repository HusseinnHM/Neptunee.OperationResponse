using System.Text.Json;
using System.Text.Json.Serialization;

namespace Neptunee.OperationsResponse.Converters;

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

internal class OperationResponseConverter<TResponse> : JsonConverter<OperationResponse<TResponse>>
{
    public override OperationResponse<TResponse>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<OperationResponse<TResponse>>(ref reader);


    public override void Write(Utf8JsonWriter writer, OperationResponse<TResponse> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (value.HasResponse)
        {
            writer.WritePropertyName(nameof(value.Response));
            JsonSerializer.Serialize(writer, value.Response, options);
        }

        writer.WriteBoolean(nameof(value.IsSuccess), value.IsSuccess);
        writer.WriteString(nameof(value.Message), value.Message ?? value.StatusCode.ToString());
        JsonSerializer.Serialize(writer, value.Errors, options);
        JsonSerializer.Serialize(writer, value.ExternalProps, options);
        writer.WriteEndObject();
    }
}