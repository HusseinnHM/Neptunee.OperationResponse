using System.Text.Json;
using System.Text.Json.Serialization;

namespace Neptunee.OResponse.Converters;

public class CombineErrorConverter : JsonConverter<IReadOnlyCollection<Error>>
{
    public override IReadOnlyCollection<Error>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<IReadOnlyCollection<Error>>(ref reader);

    public override void Write(Utf8JsonWriter writer, IReadOnlyCollection<Error> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray(nameof(Error) + "s");
        foreach (var error in value)
        {
            writer.WriteStringValue(error.ToString());
        }
        writer.WriteEndArray();
    }
}