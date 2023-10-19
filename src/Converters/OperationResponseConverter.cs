using System.Text.Json;
using System.Text.Json.Serialization;

namespace Neptunee.OResponse.Converters;

public class OperationResponseConverter : JsonConverter<OperationResponse>
{
    public override OperationResponse? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<OperationResponse>(ref reader);


    public override void Write(Utf8JsonWriter writer, OperationResponse value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteBoolean(nameof(value.IsSuccess), value.IsSuccess);
        writer.WriteString(nameof(value.Message), value.Message ?? value.StatusCode.ToString());
        JsonSerializer.Serialize(writer, value.Errors, options);
        writer.WritePropertyName(nameof(value.ExternalProps));
        JsonSerializer.Serialize(writer, value.ExternalProps, options);
        writer.WriteEndObject();
    }
}