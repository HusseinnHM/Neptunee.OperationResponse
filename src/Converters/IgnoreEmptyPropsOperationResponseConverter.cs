using System.Text.Json;
using System.Text.Json.Serialization;

namespace Neptunee.OResponse.Converters;

public class IgnoreEmptyPropsOperationResponseConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType) return false;

        return typeToConvert.GetGenericTypeDefinition() == typeof(OperationResponse<>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var responseType = typeToConvert.GetGenericArguments()[0];
        var genericResultType = typeof(IgnoreEmptyPropsOperationResponseConverter<>).MakeGenericType(responseType);
        return Activator.CreateInstance(genericResultType) as JsonConverter;
    }
}

internal class IgnoreEmptyPropsOperationResponseConverter<TResponse> : BaseOperationResponseConverter<TResponse>
{
    public override OperationResponse<TResponse>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<OperationResponse<TResponse>>(ref reader);

    public override void Write(Utf8JsonWriter writer, OperationResponse<TResponse> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        WriteBaseOperationResponse(writer, value, options);
        
        if (value.Errors.Any())
        {
            JsonSerializer.Serialize(writer, value.Errors, options);
        }

        if (value.ExternalProps.Any())
        {
            WriteExternalProps(writer,value,options);
        }

        writer.WriteEndObject();
    }
}