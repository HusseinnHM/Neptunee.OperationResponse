using System.Text.Json;

namespace Neptunee.OResponse.Converters;

public class IgnoreEmptyExternalPropsConverter : ExternalPropsConverter
{
    public override ExternalProps? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<ExternalProps>(ref reader);

    public override void Write(Utf8JsonWriter writer, ExternalProps value, JsonSerializerOptions options)
    {
        if (value.Any())
        {
            base.Write(writer, value, options);
        }
    }
}