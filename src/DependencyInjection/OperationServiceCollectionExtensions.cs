using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Neptunee.OperationResponse.Converters;

namespace Neptunee.OperationResponse.DependencyInjection;

public static class OperationServiceCollectionExtensions
{
    public static IServiceCollection AddOperationSerializerOptions(this IServiceCollection services,
        JsonConverterFactory? operationJsonConverterFactory = null,
        JsonConverter<IReadOnlyCollection<Error>>? errorJsonConverter = null,
        JsonConverter<ExternalProps>? externalPropsJsonConverter = null,
        Action<JsonSerializerOptions>? action = null)
    {
        if (operationJsonConverterFactory is not null)
        {
            OperationSettings.ResetConverterFactory(operationJsonConverterFactory);
        }
        if (errorJsonConverter is not null)
        {
            OperationSettings.ResetErrorConverter(errorJsonConverter);
        }
        if (externalPropsJsonConverter is not null)
        {
            OperationSettings.ResetExternalPropsConverter(externalPropsJsonConverter);
        }
        
        action?.Invoke(OperationSettings.JsonSerializerOptions);
        return services.Configure<JsonOptions>(o =>
        {
            foreach (var jsonConverter in OperationSettings.JsonSerializerOptions.Converters)
            {
                o.JsonSerializerOptions.Converters.Add(jsonConverter);
            }

            action?.Invoke(o.JsonSerializerOptions);
        }).Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(o =>
        {
            foreach (var jsonConverter in OperationSettings.JsonSerializerOptions.Converters)
            {
                o.SerializerOptions.Converters.Add(jsonConverter);
            }
            action?.Invoke(o.SerializerOptions);
        });
        
    }
}