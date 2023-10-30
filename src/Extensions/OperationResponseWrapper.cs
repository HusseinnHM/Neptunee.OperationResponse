using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Neptunee.OResponse;

public static class OperationResponseWrapper
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
    public static IActionResult ToIActionResult<TResponse>(this OperationResponse<TResponse> operationResponse, object? serializerSettings = null)
        => new JsonResult(operationResponse, serializerSettings)
        {
            StatusCode = (int)operationResponse.StatusCode
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
    public static async Task<IActionResult> ToIActionResultAsync<TResponse>(this Task<OperationResponse<TResponse>> task, object? serializerSettings = null)
        => (await task).ToIActionResult();

#if NET6_0_OR_GREATER
    public static IResult ToIResult<TResponse>(this OperationResponse<TResponse> operationResponse, JsonSerializerOptions? options = null)
        => Microsoft.AspNetCore.Http.Results.Json(operationResponse,
            options: options,
            statusCode: (int)operationResponse.StatusCode);

    public static async Task<IResult> ToIResultAsync<TResponse>(this Task<OperationResponse<TResponse>> task, JsonSerializerOptions? options = null)
        => (await task).ToIResult(options);
#endif

    public static OperationResponse<TResponse> ToOperationResponse<TResponse>(this IdentityResult identityResult)
        => OperationResponse<TResponse>.SuccessIf(identityResult.Succeeded, op =>
        {
            foreach (var error in identityResult.Errors.Select(e => new SpecificError(e.Code, e.Description)))
            {
                op.Error(error);
            }
        });
}