using System.Text.Json;
using System.Text.Json.Serialization;

namespace Neptunee.OResponse.Converters;

public class SplitErrorConverter : JsonConverter<IReadOnlyCollection<Error>>
{
    public override IReadOnlyCollection<Error>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<IReadOnlyCollection<Error>>(ref reader);

    public override void Write(Utf8JsonWriter writer, IReadOnlyCollection<Error> value, JsonSerializerOptions options)
    {
        var ignoreEmpty = options.GetConverter(typeof(OperationResponse)).GetType() == typeof(IgnoreEmptyPropsOperationResponseConverter);

        var errors = value.Where(e => e is not SpecificError).ToList();
        var specErrors = value.OfType<SpecificError>().ToList();

        if (!ignoreEmpty || errors.Any())
        {
            writer.WriteStartArray(nameof(Error) + "s");
            foreach (var error in errors)
            {
                writer.WriteStringValue(error.Description);
            }

            writer.WriteEndArray();
        }

        if (!ignoreEmpty || specErrors.Any())
        {
            writer.WriteStartArray(nameof(SpecificError) + "s");
            foreach (var error in specErrors)
            {
                writer.WriteStartObject();
                writer.WriteString(nameof(error.Code), error.Code);
                writer.WriteString(nameof(error.Description), error.Description);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }
    }
}