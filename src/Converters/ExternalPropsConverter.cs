using System.Text.Json;
using System.Text.Json.Serialization;

namespace Neptunee.OperationResponse.Converters;

public class ExternalPropsConverter : JsonConverter<ExternalProps>
{
    public override ExternalProps? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<ExternalProps>(ref reader);


    public override void Write(Utf8JsonWriter writer, ExternalProps value, JsonSerializerOptions options)
    {
        writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(nameof(ExternalProps)) ?? nameof(ExternalProps));
        JsonSerializer.Serialize(writer, value);
    }
}