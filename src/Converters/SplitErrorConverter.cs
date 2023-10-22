using System.Text.Json;
using System.Text.Json.Serialization;

namespace Neptunee.OResponse.Converters;

public class SplitErrorConverter : JsonConverter<IReadOnlyCollection<Error>>
{
    public override IReadOnlyCollection<Error>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<IReadOnlyCollection<Error>>(ref reader);

    public override void Write(Utf8JsonWriter writer, IReadOnlyCollection<Error> value, JsonSerializerOptions options)
    {
        WriteErrors(writer,value.Where(e => e is not SpecificError));
        WriteSpecificErrors(writer,value.OfType<SpecificError>());
    }

    protected void WriteErrors(Utf8JsonWriter writer, IEnumerable<Error> errors)
    {
        writer.WriteStartArray(nameof(Error) + "s");
        foreach (var error in errors)
        {
            writer.WriteStringValue(error.Description);
        }
        writer.WriteEndArray();
    }

    protected void WriteSpecificErrors(Utf8JsonWriter writer, IEnumerable<SpecificError> errors)
    {
        writer.WriteStartArray(nameof(SpecificError) + "s");
        foreach (var error in errors)
        {
            writer.WriteStartObject();
            writer.WriteString(nameof(error.Code), error.Code);
            writer.WriteString(nameof(error.Description), error.Description);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }

}