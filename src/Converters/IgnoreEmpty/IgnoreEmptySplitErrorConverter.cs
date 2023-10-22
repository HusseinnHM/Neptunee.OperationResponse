using System.Text.Json;

namespace Neptunee.OResponse.Converters;

public class IgnoreEmptySplitErrorConverter : SplitErrorConverter
{
    public override void Write(Utf8JsonWriter writer, IReadOnlyCollection<Error> value, JsonSerializerOptions options)
    {
        var errors = value.Where(e => e is not SpecificError).ToList();
        var specErrors = value.OfType<SpecificError>().ToList();

        if (errors.Any())
        {
            WriteErrors(writer,errors);
        }

        if (specErrors.Any())
        {
            WriteSpecificErrors(writer,specErrors);
        }
    }
}