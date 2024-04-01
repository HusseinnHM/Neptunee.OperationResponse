using System.Text.Json;
using System.Text.Json.Serialization;

namespace Neptunee.OperationResponse.Converters;

public class SplitErrorConverter : JsonConverter<IReadOnlyCollection<Error>>
{
    public override IReadOnlyCollection<Error>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<IReadOnlyCollection<Error>>(ref reader);

    public override void Write(Utf8JsonWriter writer, IReadOnlyCollection<Error> value, JsonSerializerOptions options)
    {
        WriteErrors(writer, value.Where(e => e is not SpecificError), options);
        WriteSpecificErrors(writer, value.OfType<SpecificError>(), options);
    }

    protected void WriteErrors(Utf8JsonWriter writer, IEnumerable<Error> errors, JsonSerializerOptions options)
    {
        writer.WriteStartArray(options.PropertyNamingPolicy?.ConvertName(nameof(Error) + "s") ?? nameof(Error) + "s");
        foreach (var error in errors)
        {
            writer.WriteStringValue(error.Description);
        }

        writer.WriteEndArray();
    }

    protected void WriteSpecificErrors(Utf8JsonWriter writer, IEnumerable<SpecificError> errors, JsonSerializerOptions options)
    {
        writer.WriteStartArray(options.PropertyNamingPolicy?.ConvertName(nameof(Error) + "s") ?? nameof(Error) + "s");
        foreach (var error in errors)
        {
            writer.WriteStartObject();
            writer.WriteString(options.PropertyNamingPolicy?.ConvertName(nameof(error.Code)) ?? nameof(error.Code), error.Code);
            writer.WriteString(options.PropertyNamingPolicy?.ConvertName(nameof(error.Description)) ?? nameof(error.Description), error.Description);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }
}