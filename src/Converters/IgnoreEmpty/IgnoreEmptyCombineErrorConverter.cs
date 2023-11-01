using System.Text.Json;

namespace Neptunee.OperationResponse.Converters;

public class IgnoreEmptyCombineErrorConverter : CombineErrorConverter
{
    public override void Write(Utf8JsonWriter writer, IReadOnlyCollection<Error> value, JsonSerializerOptions options)
    {
        if (value.Any())
        {
            base.Write(writer,value,options);
        }
    }
}