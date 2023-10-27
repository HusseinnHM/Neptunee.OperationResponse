using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Neptunee.OResponse.Converters;

namespace Neptunee.OResponse.DependencyInjection;

public static class OperationResponseCollectionExtensions
{
    public static IServiceCollection AddOperationResponseSerializerOptions(this IServiceCollection services,
        JsonConverterFactory? operationResponseJsonConverterFactory = null,
        JsonConverter<IReadOnlyCollection<Error>>? errorJsonConverter = null,
        JsonConverter<ExternalProps>? externalPropsJsonConverter = null,
        Action<JsonSerializerOptions>? action = null)
    {
        if (operationResponseJsonConverterFactory is not null)
        {
            OperationResponseSettings.ResetConverterFactory(operationResponseJsonConverterFactory);
        }
        if (errorJsonConverter is not null)
        {
            OperationResponseSettings.ResetErrorConverter(errorJsonConverter);
        }
        if (externalPropsJsonConverter is not null)
        {
            OperationResponseSettings.ResetExternalPropsConverter(externalPropsJsonConverter);
        }
        
        action?.Invoke(OperationResponseSettings.JsonSerializerOptions);
        return services.Configure<JsonOptions>(o =>
        {
            foreach (var jsonConverter in OperationResponseSettings.JsonSerializerOptions.Converters)
            {
                o.JsonSerializerOptions.Converters.Add(jsonConverter);
            }

            action?.Invoke(o.JsonSerializerOptions);
        }).Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(o =>
        {
            foreach (var jsonConverter in OperationResponseSettings.JsonSerializerOptions.Converters)
            {
                o.SerializerOptions.Converters.Add(jsonConverter);
            }
            action?.Invoke(o.SerializerOptions);
        });
        
    }
}