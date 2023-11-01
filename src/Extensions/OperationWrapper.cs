using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Neptunee.OperationResponse;

public static class OperationWrapper
{
    /// <param name="serializerSettings">
    /// The serializer settings to be used by the formatter.
    /// <para>
    /// When using <c>System.Text.Json</c>, this should be an instance of <see cref="T:System.Text.Json.JsonSerializerOptions" />.
    /// </para>
    /// <para>
    /// When using <c>Newtonsoft.Json</c>, this should be an instance of <c>JsonSerializerSettings</c>.
    /// </para>
    /// </param>
    public static IActionResult ToIActionResult<TResponse>(this Operation<TResponse> operation, object? serializerSettings = null)
        => new JsonResult(operation, serializerSettings)
        {
            StatusCode = (int)operation.StatusCode
        };

    /// <param name="serializerSettings">
    /// The serializer settings to be used by the formatter.
    /// <para>
    /// When using <c>System.Text.Json</c>, this should be an instance of <see cref="T:System.Text.Json.JsonSerializerOptions" />.
    /// </para>
    /// <para>
    /// When using <c>Newtonsoft.Json</c>, this should be an instance of <c>JsonSerializerSettings</c>.
    /// </para>
    /// </param>
    public static async Task<IActionResult> ToIActionResultAsync<TResponse>(this Task<Operation<TResponse>> task, object? serializerSettings = null)
        => (await task).ToIActionResult();

#if NET6_0_OR_GREATER
    public static IResult ToIResult<TResponse>(this Operation<TResponse> operation, JsonSerializerOptions? options = null)
        => Microsoft.AspNetCore.Http.Results.Json(operation,
            options: options,
            statusCode: (int)operation.StatusCode);

    public static async Task<IResult> ToIResultAsync<TResponse>(this Task<Operation<TResponse>> task, JsonSerializerOptions? options = null)
        => (await task).ToIResult(options);
#endif

    public static Operation<TResponse> ToOperation<TResponse>(this IdentityResult identityResult)
        => Operation<TResponse>.SuccessIf(identityResult.Succeeded, op =>
        {
            foreach (var error in identityResult.Errors.Select(e => new SpecificError(e.Code, e.Description)))
            {
                op.Error(error);
            }
        });
    
}