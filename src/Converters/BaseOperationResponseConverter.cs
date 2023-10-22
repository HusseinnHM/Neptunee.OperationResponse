using System.Text.Json;
using System.Text.Json.Serialization;

namespace Neptunee.OResponse.Converters;

internal abstract class BaseOperationResponseConverter<TResponse>: JsonConverter<OperationResponse<TResponse>>
{
    protected void WriteBaseOperationResponse(Utf8JsonWriter writer,OperationResponse<TResponse> value,JsonSerializerOptions options)
    {
        if (value.HasResponse)
        {
            writer.WritePropertyName(nameof(value.Response));
            JsonSerializer.Serialize(writer, value.Response, options);
        }
        writer.WriteBoolean(nameof(value.IsSuccess), value.IsSuccess);
        writer.WriteString(nameof(value.Message), value.Message ?? value.StatusCode.ToString());
    }   

    protected void WriteExternalProps(Utf8JsonWriter writer,OperationResponse<TResponse> value,JsonSerializerOptions options)
    {
        writer.WritePropertyName(nameof(value.ExternalProps));
        JsonSerializer.Serialize(writer, value.ExternalProps, options);
    }
}