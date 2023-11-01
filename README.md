# Neptunee.OperationResponse

![](https://img.shields.io/nuget/dt/Neptunee.OperationResponse)   [![](https://img.shields.io/nuget/v/Neptunee.OperationResponse)](https://www.nuget.org/packages/Neptunee.OperationResponse)

*Neptunee.OperationResponse* streamlines how you handle the response in your application. It eliminates complex if/else statements and offers multi-status support, a user-friendly API,validations checks, JSON compatibility, sync/async operations, and more. It's a clean way to manage the outcomes of your backend.

<p align="center">
<img width="23%" src="icon.png"  alt="icon"/>

## Overview

The code Inside your Controllers, ApiEndpoints or Command/Query Handlers will be more cleaner and readable.

```csharp
Operation<Response>
    .SuccessIf(request.Age >= 18, MyValidationErrors.AgeUnder18) // AgeUnder18 is new Error("Age must be 18 or older")
    .OrFailureIf(request.Amount > 1000, MyValidationErrors.AmountOverMax) // AmountOverMax is new SpecificError("Amount","Over maximum") 
    .SetMessageOnFailure("One or more validation errors occur")
    .SetMessageOnSuccess("It's work")
    .OnSuccess(op =>
    {
        //your logic
    })
    .OnFailure(op => _logger.LogError("the logging"))
```

The `Operation<Response>` Converted to `IActionResult` For actions in the controllers or `IResult` for the endpoint.

```json
{
  "IsSuccess": true,
  "Message": "It's work",
  "Response": {
    // your response object
  }
}
```

```json
{
  "IsSuccess": false,
  "Message": "One or more validation errors occur",
  "Errors": [
    "Age must be 18 or older"
  ],
  "SpecificErrors": [
    {
      "Code": "Amount",
      "Description": "Over maximum"
    }
  ]
}
```

## Schema

<details>
  <summary>Operation&lt;TResponse&gt;</summary>

#### Properties

| Property           | Type                         | Description                                                                                                                                                                                                                                                      |
|--------------------|------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Message**        | `string?`                    | An optional message providing additional information or context related to the operation.<br/> The default serialized message is the name of `StatusCode` (e.g., Ok, BadRequest, etc.).                                                                          |                                 |
| **Response**       | `TResponse?`                 | The actual response data of the operation.                                                                                                                                                                                                                       |
| **Errors**         | `IReadOnlyCollection<Error>` | List of ([Error](src/Errors/Error.cs) or [SpecificError](src/Errors/SpecificError.cs)) associated with the operation.                                                                                                                                            |
| **HttpStatusCode** | `HttpStatusCode`             | [HttpStatusCode](https://github.com/dotnet/aspnetcore/blob/main/src/Http/Http.Abstractions/src/StatusCodes.cs) enum represents the HTTP status code returned by the operation.<br/> The default status `Ok` if errors property is empty or will be `BadRequest`. |
| **ExternalProps**  | `ExternalProps`              | [ExternalProps](src/ExternalProps.cs) object provides external properties provided based on the operation, actually it's a `Dictionary<string,string>`.                                                                                                          |
| **IsSuccess**      | `bool`                       | Indicates if the operation was successful.                                                                                                                                                                                                                       |
| **IsFailure**      | `bool`                       | Indicates if the operation has failed.                                                                                                                                                                                                                           |

#### Methods

| Method                                                        | Description                                               |
|---------------------------------------------------------------|-----------------------------------------------------------|
| `SetResponse(TResponse? response)`                            | Sets the response of the operation.                       |
| `SetMessage(string? message, bool overwrite = false)`         | Sets the message related to the operation.                |
| `SetMessageOnSuccess(string message, bool overwrite = false)` | Sets the message when operation is success.               |
| `SetMessageOnFailure(string message, bool overwrite = false)` | Sets the message when operation is failure.               |
| `SetStatusCode(HttpStatusCode statusCode)`                    | Sets the HTTP status code.                                |
| `Error(Error error)`                                          | Adds an error to the operation.                           |
| `ExternalProp<TValue>(string key, TValue value)`              | Adds an external property to the operation.               |
| `OnSuccess(Action<Operation<TResponse>> action)`              | Executes an action when operation is success.             |
| `OnSuccessAsync(Func<Operation<TResponse>, Task> task)`       | Asynchronously executes a task when operation is success. |
| `OnFailure(Action<Operation<TResponse>> action)`              | Executes an action when operation is failure.             |
| `OnFailureAsync(Func<Operation<TResponse>, Task> task)`       | Asynchronously executes a task when operation is failure. |

#### Static Factory Methods

| Method                               | Description                                                              |
|--------------------------------------|--------------------------------------------------------------------------|
| `Unknown()`                          | Creates an `Operation<TResponse>` with an unknown status.                |
| `Ok(string? message = null)`         | Creates a successful `Operation<TResponse>` with an optional message.    |
| `BadRequest(string? message = null)` | Creates a failed `Operation<TResponse>` with an optional message.        |
| `Result(Result result)`              | Creates an `Operation<TResponse>` from an `Result`.                      |
| `Result(Result<TResponse> result)`   | Creates an `Operation<TResponse>` from an `Result` with a response data. |

</details>
<details>
    <summary>SuccessIf.Sync</summary>

<br>*Partial class* `Operation<TResponse>`.

#### Methods

| Method                                                                     | Description                                                                                                                                                                                                                       |
|----------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `OrSuccessIf(bool predicate, Action<Operation<TResponse>> onFalse)`        | Executes the provided action `onFalse` on the current `Operation<TResponse>` if the provided boolean `predicate` is `false`, otherwise do nothing.                                                                                |
| `OrSuccessIf(bool predicate, Error errorOnFalse)`                          | Adds the provided error `errorOnFalse` to the current `Operation<TResponse>` if the provided boolean `predicate` is `false`, otherwise do nothing.                                                                                |
| `OrSuccessIf(Func<bool> predicate, Action<Operation<TResponse>> onFalse)`  | Executes the provided action `onFalse` on the current `Operation<TResponse>` if the boolean result of the provided predicate function is `false`, otherwise do nothing.                                                           |
| `OrSuccessIf(Func<bool> predicate, Error errorOnFalse)`                    | Adds the provided error `errorOnFalse` to the current `Operation<TResponse>` if the boolean result of the provided predicate function is `false`, otherwise do nothing.                                                           |
| `OrIf(Result result)`                                                      | Modifies the current `Operation<TResponse>` based on the properties of the provided `result`.                                                                                                                                     |
| `OrIf(Func<Result> result)`                                                | Modifies the current `Operation<TResponse>` based on the properties of the provided `result` obtained through the function.                                                                                                       |
| `AndSuccessIf(Func<bool> predicate, Action<Operation<TResponse>> onFalse)` | If the current `Operation<TResponse>` is still a success, executes the provided action `onFalse` on the current `Operation<TResponse>` if the boolean result of the provided predicate function is `false`, otherwise do nothing. |
| `AndSuccessIf(Func<bool> predicate, Error errorOnFalse)`                   | If the current `Operation<TResponse>` is still a success, adds the provided error `errorOnFalse` to the current `Operation<TResponse>` if the boolean result of the provided predicate function is `false`, otherwise do nothing. |
| `AndIf(Func<Result> result)`                                               | If the current `Operation<TResponse>` is still a success, modifies it based on the properties of the provided `result` obtained through the function.                                                                             |

#### Static Factory Methods

| Method                                                                  | Description                                                                                                                                                                                                                |
|-------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `SuccessIf(bool predicate, Action<Operation<TResponse>> onFalse)`       | Creates a new unknown `Operation<TResponse>`, then executes the provided action `onFalse` on the current `Operation<TResponse>` if the provided boolean `predicate` is `false`, otherwise do nothing.                      |
| `SuccessIf(bool predicate, Error errorOnFalse)`                         | Creates a new unknown `Operation<TResponse>`, then adds the provided error `errorOnFalse` to the current `Operation<TResponse>` if the provided boolean `predicate` is `false`, otherwise do nothing.                      |
| `SuccessIf(Func<bool> predicate, Action<Operation<TResponse>> onFalse)` | Creates a new unknown `Operation<TResponse>`, then executes the provided action `onFalse` on the current `Operation<TResponse>` if the boolean result of the provided predicate function is `false`, otherwise do nothing. |
| `SuccessIf(Func<bool> predicate, Error errorOnFalse)`                   | Creates a new unknown `Operation<TResponse>`, then adds the provided error `errorOnFalse` to the current `Operation<TResponse>` if the boolean result of the provided predicate function is `false`, otherwise do nothing. |
| `If(Result result)`                                                     | Creates a new unknown `Operation<TResponse>`, then modifies the current `Operation<TResponse>` based on the properties of the provided `result`.                                                                           |

</details>
<details>
    <summary>SuccessIf.Async</summary>

<br>*Asynchronous of* `SuccessIf.Sync`.

#### Methods

| Method                                                                                |
|---------------------------------------------------------------------------------------|
| `OrSuccessIfAsync(Func<Task<bool>> predicate, Action<Operation<TResponse>> onFalse)`  |
| `OrSuccessIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)`                    |
| `OrIfAsync(Func<Task<Result>> result)`                                                |
| `AndSuccessIfAsync(Func<Task<bool>> predicate, Action<Operation<TResponse>> onFalse)` |
| `AndSuccessIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)`                   |
| `AndIfAsync(Func<Task<Result>> result)`                                               |

#### Static Factory Methods

| Method                                                                             |
|------------------------------------------------------------------------------------|
| `SuccessIfAsync(Func<Task<bool>> predicate, Action<Operation<TResponse>> onFalse)` |
| `SuccessIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)`                   |

</details>

<details>
    <summary>FailureIf.Sync</summary>

<br>*Partial class* `Operation<TResponse>`.

#### Methods

| Method                                                                    | Description                                                                                                                                                                                                                     |
|---------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `OrFailureIf(bool predicate, Action<Operation<TResponse>> onTrue)`        | Executes the provided action `onTrue` on the current `Operation<TResponse>` if the provided boolean `predicate` is `true`, otherwise do nothing.                                                                                |
| `OrFailureIf(bool predicate, Error errorOnTrue)`                          | Adds the provided error `errorOnTrue` to the current `Operation<TResponse>` if the provided boolean `predicate` is `true`, otherwise do nothing.                                                                                |
| `OrFailureIf(Func<bool> predicate, Action<Operation<TResponse>> onTrue)`  | Executes the provided action `onTrue` on the current `Operation<TResponse>` if the boolean result of the provided predicate function is `true`, otherwise do nothing.                                                           |
| `OrFailureIf(Func<bool> predicate, Error errorOnTrue)`                    | Adds the provided error `errorOnTrue` to the current `Operation<TResponse>` if the boolean result of the provided predicate function is `true`, otherwise do nothing.                                                           |
| `AndFailureIf(Func<bool> predicate, Action<Operation<TResponse>> onTrue)` | If the current `Operation<TResponse>` is still a success, executes the provided action `onTrue` on the current `Operation<TResponse>` if the boolean result of the provided predicate function is `true`, otherwise do nothing. |
| `AndFailureIf(Func<bool> predicate, Error errorOnTrue)`                   | If the current `Operation<TResponse>` is still a success, adds the provided error `errorOnTrue` to the current `Operation<TResponse>` if the boolean result of the provided predicate function is `true`, otherwise do nothing. |

#### Static Factory Methods

| Method                                                                 | Description                                                                                                                                                                                                              |
|------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `FailureIf(bool predicate, Action<Operation<TResponse>> onTrue)`       | Creates a new unknown `Operation<TResponse>`, then executes the provided action `onTrue` on the current `Operation<TResponse>` if the provided boolean `predicate` is `true`, otherwise do nothing.                      |
| `FailureIf(bool predicate, Error errorOnTrue)`                         | Creates a new unknown `Operation<TResponse>`, then adds the provided error `errorOnTrue` to the current `Operation<TResponse>` if the provided boolean `predicate` is `true`, otherwise do nothing.                      |
| `FailureIf(Func<bool> predicate, Action<Operation<TResponse>> onTrue)` | Creates a new unknown `Operation<TResponse>`, then executes the provided action `onTrue` on the current `Operation<TResponse>` if the boolean result of the provided predicate function is `true`, otherwise do nothing. |
| `FailureIf(Func<bool> predicate, Error errorOnTrue)`                   | Creates a new unknown `Operation<TResponse>`, then adds the provided error `errorOnTrue` to the current `Operation<TResponse>` if the boolean result of the provided predicate function is `true`, otherwise do nothing. |

</details>

<details>
    <summary>FailureIf.Async</summary>

<br>*Asynchronous of* `FailureIf.Sync`.

#### Methods

| Method                                                                               |
|--------------------------------------------------------------------------------------|
| `OrFailureIfAsync(Func<Task<bool>> predicate, Action<Operation<TResponse>> onTrue)`  |
| `OrFailureIfAsync(Func<Task<bool>> predicate, Error errorOnTrue)`                    |
| `AndFailureIfAsync(Func<Task<bool>> predicate, Action<Operation<TResponse>> onTrue)` |
| `AndFailureIfAsync(Func<Task<bool>> predicate, Error errorOnTrue)`                   |

#### Static Factory Methods

| Method                                                                            |
|-----------------------------------------------------------------------------------|
| `FailureIfAsync(Func<Task<bool>> predicate, Action<Operation<TResponse>> onTrue)` |
| `FailureIfAsync(Func<Task<bool>> predicate, Error errorOnFalse)`                  |

</details>
<details>
  <summary>NoResponse</summary>

<br>The [NoResponse](src/NoResponse.cs) is abstract record to define that operation will not return actual response data.

</details>
<details>
  <summary>Error</summary>

<br>The `Error` record represents a error with a textual description. It is commonly used to provide human-readable error messages.

#### Properties

| Property        | Type     | Description                         |
|-----------------|----------|-------------------------------------|
| **Description** | `string` | A textual description of the error. |

#### Conversion Operators

| Method                                        | Description                                                               |
|-----------------------------------------------|---------------------------------------------------------------------------|
| `implicit operator Error(string description)` | Implicitly converts a string to an `Error` with the provided description. |

</details>

<details>
  <summary>SpecificError</summary>

<br>*Inherits* `Error`.
<br>Representing an error with both a code and a description. It is commonly used to provide more specific error information, such as in any prop or field it happened.

#### Properties

| Property        | Type     | Description                         |
|-----------------|----------|-------------------------------------|
| **Code**        | `string` | A code associated with the error.   |
| **Description** | `string` | A textual description of the error. |

</details>
<details>
  <summary>Result</summary>

<br>Use the `Result` class in scenarios where you need to handle logic before passing the relevant information to the `Operation<TResponse>`.
<br>It used as an intermediary layer that can encapsulate and communicate information between services or methods.

#### Properties

| Property          | Type                         | Description                                                                                                                       |
|-------------------|------------------------------|-----------------------------------------------------------------------------------------------------------------------------------|
| **StatusCode**    | `HttpStatusCode`             | The HTTP status code associated with the result.                                                                                  |
| **Error**         | `Error?`                     | An optional error associated with the result.                                                                                     |
| **Message**       | `string?`                    | An optional message providing additional information or context related to the result.                                            |
| **ExternalProps** | `Dictionary<string, string>` | Additional external properties provided as key-value pairs.                                                                       |
| **IsSuccess**     | `bool`                       | Indicates if the result represents a successful operation, typically when the `StatusCode` falls within the range of 200-299.     |
| **IsFailure**     | `bool`                       | Indicates if the result represents a failed operation, typically when the `StatusCode` does not fall within the range of 200-299. |

#### Methods

| Method                     | Description                                                             |
|----------------------------|-------------------------------------------------------------------------|
| `To<TValue>()`             | Converts the `Result` to an `Result<TValue>` without providing a value. |
| `To<TValue>(TValue value)` | Converts the `Result` to an `Result<TValue>` and provides a value.      |

#### Static Factory Methods

| Method                                                                                                                          | Description                                                                         |
|---------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------|
| `With(HttpStatusCode statusCode, Error? error = null, string? message = null, Dictionary<string, string> externalProps = null)` | Creates a new `Result` with the specified properties.                               |
| `Ok(string? message = null, Dictionary<string, string> externalProps = null)`                                                   | Creates a successful `Result` with an optional message and external properties.     |
| `BadRequest(Error? error = null, string? message = null, Dictionary<string, string> externalProps = null)`                      | Creates a failed `Result` with an optional error, message, and external properties. |

</details>
<details>
  <summary>Result&lt;TValue&gt;</summary>

<br>*Inherits* `Result`.
<br>Use `Result<TValue>` in case there are `TValue` will returned.

#### Properties

| Property     | Type     | Description                                    |
|--------------|----------|------------------------------------------------|
| **Value**    | `TValue` | representing the actual return data.           |
| **HasValue** | `bool`   | Indicates if the `Value` has a non-null value. |

#### Methods

| Method             | Description                                                                                   |
|--------------------|-----------------------------------------------------------------------------------------------|
| `ValueOrDefault()` | Retrieves the `Value` if it has a non-null value, or returns `null` if the `Value` is `null`. |

#### Conversion Operators

| Operator                                          | Description                                                                                    |
|---------------------------------------------------|------------------------------------------------------------------------------------------------|
| `implicit operator TValue(Result<TValue> result)` | Implicitly converts an `Result<TValue>` to its `Value`.                                        |
| `implicit operator Result<TValue>(TValue value)`  | Implicitly converts a `TValue` to a successful `Result<TValue>` containing the provided value. |

</details>
<details>
  <summary>OperationSettings</summary>

<br>Provides a central place to manage and configure settings related to the serialization of `Operation<TResponse>` objects.

#### Properties

| Property                  | Type                    | Description                                                                             |
|---------------------------|-------------------------|-----------------------------------------------------------------------------------------|
| **JsonSerializerOptions** | `JsonSerializerOptions` | The JSON serialization options used for custom serialization of `Operation<TResponse>`. |

#### Methods

| Method                                                                     | Description                                                   |
|----------------------------------------------------------------------------|---------------------------------------------------------------|
| `ResetConverterFactory(JsonConverterFactory converterFactory)`             | Resets the JSON converter factory for `Operation<TResponse>`. |
| `ResetConverterFactory<TJsonConverter>()`                                  | Resets the JSON converter factory for `Operation<TResponse>`. |
| `ResetErrorConverter(JsonConverter<IReadOnlyCollection<Error>> converter)` | Resets the JSON converter for `IReadOnlyCollection<Error>`.   |
| `ResetErrorConverter<TJsonConverter>()`                                    | Resets the JSON converter for `IReadOnlyCollection<Error>`.   |
| `ResetExternalPropsConverter(JsonConverter<ExternalProps> converter)`      | Resets the JSON converter for `ExternalProps`.                |
| `ResetExternalPropsConverter<TJsonConverter>()`                            | Resets the JSON converter for `ExternalProps`.                |

Use the `OperationSettings` class to configure the JSON serialization options for `Operation<TResponse>` and to
manage custom JSON converters.

</details>

<details>
  <summary>OperationServiceCollectionExtensions</summary>

<br>Offers extension methods for configuring the `Operation<TResponse>` JSON serialization options within MVC and HTTP serialization options in ASP.NET Core.

#### Methods

| Method                                                                                                                                                                                                                                                                           | Description                                                                                                         |
|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------|
| `AddOperationSerializerOptions(JsonConverterFactory? operationJsonConverterFactory = null, JsonConverter<IReadOnlyCollection<Error>>? errorJsonConverter = null, JsonConverter<ExternalProps>? externalPropsJsonConverter = null, Action<JsonSerializerOptions>? action = null)` | Configures JSON serialization options for `Operation<TResponse>` and related objects within the service collection. |

Use the `OperationServiceCollectionExtensions` class when you want to configure JSON serialization options for your ASP.NET Core application, especially when working with `Operation<TResponse>` and related types.

</details>
<details>
  <summary>OperationWrapper</summary>

| Method                                                                                                          | Description                                                                                            |
|-----------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------|
| `ToIActionResult<TResponse>(this Operation<TResponse> Operation<TResponse>, object? serializerSettings = null)` | Converts an `Operation<TResponse>` to a `JsonResult`.                                                  |
| `ToIActionResultAsync<TResponse>(this Task<Operation<TResponse>> task, object? serializerSettings = null)`      | Asynchronously converts an `Operation<TResponse>` to a `JsonResult`.                                   |
| `ToIResult<TResponse>(this Operation<TResponse> Operation<TResponse>,JsonSerializerOptions? options = null)`    | Converts an `Operation<TResponse>` to a `Results.Json` (supported in .NET 6.0 or greater).             |
| `ToIResultAsync<TResponse>(this Task<Operation<TResponse>> task,JsonSerializerOptions? options = null)`         | Asynchronously converts `Operation<TResponse>` to a `Results.Json` (supported in .NET 6.0 or greater). |
| `ToOperation<TResponse>(this IdentityResult identityResult)`                                                    | Converts an `IdentityResult` to an `Operation<TResponse>`.                                             |

</details>

## Documentation

<details>
  <summary>How to validate and set errors</summary>

<br>There are bunch of ways to validate your operation (check out all methods in [Schema](#schema)).
<br>For defining errors there are 3 ways:

- `Error(string Description)` record.
- `SpecificError(string Code,string Description)` record inherits `Error(string Description)`.
- `string` implicit convert to `Error(string Description)`.

All errors set to `List<Error>`.

```csharp
public class SampleRequestHandler : IRequestHandler<Request,Operation<NoResponse>>
{
    // ctor and injections
    
    public async Task<Operation<NoResponse>> HandelAsync(Request request)
        => await Operation
            .SuccessIf(request.Prop == true, "the error") // way 1
            .OrFailureIf(() => request.Prop == true, new Error("the error")) // way 2
            .OrFailureIf(request.Prop == true, new SpecificError("the code", "description")) // way 3
            .AndFailureIfAsync(async () => await _service.IsGoneAsync(), op =>
            {
                //case: logic if this error occur 
                op.SetStatusCode(HttpStatusCode.Gone);
                op.Error(/* chose way you like */);
            })
            .OnSuccessAsync(op =>
            {
                // your logic
            });
}
```

The deference between `Or` / `And` methods is the `And` methods check if `Operation<TResponse>` is still success before execute, otherwise will skip.
<br>
So you may use `And` in case you have expensive check that you only want to check when everything else is alright.
</details>
<details>
  <summary>How to set status code</summary>

<br>You can use `SetStatusCode()` method to set custom status code you need.
<br>No need to set the **Ok** & **BadRequest** status because will automatically set if the `Errors` property was empty or not.

```csharp
    public Operation<NoResponse> Handel(Request request)
        => Operation<NoResponse>
            .IfSuccsus(/* passing params */)
            .OnSuccess(op =>
            {
                op.SetStatusCode(HttpStatusCode.Created);
                // logic
            });
```

```csharp
    public Operation<NoResponse> Handel(Request request)
    {
        // logic
        retrun Operation<NoResponse>.SetStatusCode(HttpStatusCode.Continue)
    } 
```

</details>
<details>
  <summary>How to set message</summary>

```diff
public class SampleRequestHandler : IRequestHandler<Request,Operation>
{
    // ctor and injections
    
    public async Task<Operation<NoResponse>> HandelAsync(Request request)
        => await Operation
            .SuccessIf(request.Prop == true, "the error") // way 1
            .OrFailureIf(request.Prop == true, new Error("the error")) // way 2
            .OrFailureIf(request.Prop == true, new SpecificError("the code", "description")) // way 3
            .AndFailureIfAsync(async () => await _service.IsGoneAsync(), op =>
            {
                // case: logic if this error occur 
                op.SetStatusCode(HttpStatusCode.Gone);
                op.Error(/* chose way you like */);
            })
+           .SetMessageOnSuccess("your succses message") // way 1
+           .SetMessageOnFailure("your failure message") // way 2
+           .SetMessage("your message") // way 3
            .OnSuccessAsync(op =>
            {
                // your logic
            });
}
```

This setters check if the message is null first, so like the example above the `SetMessage()` will not execute but the
setters has optional parameter `bool overwrite = false` can make it execute (`SetMessage("your message",true)`) in your rare case.
</details>

<details>
  <summary>How to set response</summary>

<br>You can use `SetResponse()` method to set the actual data of the operation or just return the response that will implicitly convert to `Operation<TResponse>` with `200` as status code and `OK` as message.

```csharp
    public Operation<Response> Handel(Request request)
        => Operation<Response>
            .IfSuccsus(/* passing params */)
            .OnSuccess(op =>
            {
                // logic
                op.SetResponse(new Response(/* passing params */));
            });
```

```csharp
    public Operation<Response> Handel(Request request)
    {
        // logic
        retrun new Response(/* passing params */);
    } 
```

</details>

<details>
  <summary>How to serialize</summary>

<br>When serialize `Operation<TResponse>` must use `JsonSerializerOptions` in [OperationSettings](src/OperationSettings.cs).

```csharp
JsonSerializer.Serialize(operation, OperationSettings.JsonSerializerOptions);
```

To configure these options for MVC and HTTP serialization in ASP.NET Core. In other words, to use them when calling `ToIActionResult()` or `ToIResult()` should use:

```csharp
bulider.Services.AddOperationSerializerOptions();
```

You can also change the default converters or options using `OperationSettings` methods, check the schema [here](#methods-7).

```csharp
OperationSettings.ResetErrorConverter<IgnoreEmptyCombineErrorConverter>();
```

or pass to optional parameters in `AddOperationSerializerOptions()`:

```csharp
bulider.Services.AddOperationSerializerOptions(externalPropsJsonConverter: new ExternalPropsConverter());
```

The `JsonSerializerOptions` has 3 JSON converts for *Operation<TResponse>*, *ExternalProps* and *Errors*.

P.S: can check the available JSON converters in ****Custom JSON converters**** bellow.
</details>

<details>
  <summary>Custom JSON converters</summary>

<br>The custom JSON converters divided to:

- Operation<TResponse>
    - [OperationConverter with Factory](src/Converters/OperationConverter.cs) *(the default)*:
        - `IsSuccess` : serialize to **boolean**.
        - `Message` : serialize to **string** if is not null otherwise to name of status code.
        - `Response` : serialize to **object** if is not null otherwise ignore it.
        - `Errors` : serialize depends on the custom converter provided *(the default: `IgnoreEmptySplitErrorConverter`)*.
        - `ExtrenalProps` : serialize depends on the custom converter provided *(the default: `IgnoreEmptyExternalPropsConverter`)*.
        ```json
        {
          "IsSuccess": true,
          "Message": "OK"
        }
        ```
        ```json
        {
          "IsSuccess": false,
          "Message": "BadRequest",
          "Errors": [
            "the error"
          ]
        }
        ```
        ```json
        {
          "IsSuccess": true,
          "Message": "Custom message",
          "Response": {
            // The actual response object
          }
        }
      ```
- ExternalProps
    - [ExternalPropsConverter](src/Converters/ExternalPropsConverter.cs):
        - `ExternalProps` : serialize to **object**.
        ```json
        {
          "IsSuccess": true,
          "Message": "OK",
          "ExternalProps": {
            "key": "value"
          }
        }
        ```
        ```json
        {
          "IsSuccess": true,
          "Message": "OK",
          "ExternalProps": {}
        }
        ```
    - [IgnoreEmptyExternalPropsConverter](src/Converters/IgnoreEmpty/IgnoreEmptyExternalPropsConverter.cs) *(the default)*:
      ###### Inherits ExternalPropsConverter
        - `ExternalProps`: serialize to **object** if are *not empty* otherwise **ignore** it.
        ```json
        {
          "IsSuccess": true,
          "Message": "OK",
          "ExternalProps": {
            "key": "value"
          }
        }
        ```
        ```json
        {
          "IsSuccess": true,
           "Message": "OK"
        }
        ```
- Errors
    - [SplitErrorConverter](src/Converters/SplitErrorConverter.cs):
        - `Errors` : serialize the *Error(string Description)* to **list of strings**.
        - `SpecificErrors` : serialize the *SpecificError(string Code,string Description)* to **list of objects**.
        ```json
        {
          "IsSuccess": false,
          "Message": "BadRequest",
          "Errors": [
            "the error"
          ],
          "SpecificErrors": [
            {
              "Code": "The Code",
              "Description": "The Description"
            }
          ]
        }
        ```
        ```json
        {
          "IsSuccess": true,
          "Message": "OK",
          "Errors": [],
          "SpecificErrors": []
        }
        ```
    - [CombineErrorConverter](src/Converters/CombineErrorConverter.cs):
        - `Errors` : serialize to **list of strings** by converting `SpecificError(string Code,string Description)` object to `$"{Code} : {Description}"` string.
        ```json
        {
          "IsSuccess": false,
          "Message": "BadRequest",
          "Errors": [
            "the error",
            "The Code : The Description"
          ]
        }
        ```
        ```json
        {
          "IsSuccess": true,
          "Message": "OK",
          "Errors": []
        }
        ``` 
    - [IgnoreEmptySplitErrorConverter](src/Converters/IgnoreEmpty/IgnoreEmptySplitErrorConverter.cs) *(the default)*:
      ###### Inherits SplitErrorConverter
        - `Errors` : serialize the *Error(string Description)* to **list of strings** if are *not empty* otherwise **ignore** it..
        - `SpecificErrors` : serialize the *SpecificError(string Code,string Description)* to **list of objects** if are *not empty* otherwise **ignore** it.
         ```json
        {
          "IsSuccess": false,
          "Message": "BadRequest",
          "Errors": [
            "the error"
          ],
          "SpecificErrors": [
            {
              "Code": "The Code",
              "Description": "The Description"
            }
          ]
        }
        ```
        ```json
        {
          "IsSuccess": false,
          "Message": "BadRequest",
          "Errors": [
            "the error"
          ]
        }
        ```
        ```json
        {
          "IsSuccess": true,
          "Message": "OK"
        }
        ```
    - [IgnoreEmptyCombineErrorConverter](src/Converters/IgnoreEmpty/IgnoreEmptyCombineErrorConverter.cs):
      ###### Inherits CombineErrorConverter
        - `Errors` : serialize to **list of strings** by converting `SpecificError(string Code,string Description)` object to `$"{Code} : {Description}"` string if are *not empty* otherwise **ignore** it.
        ```json
        {
          "IsSuccess": false,
          "Message": "BadRequest",
          "Errors": [
            "The Code : The Description"
          ]
        }
        ```
        ```json
        {
          "IsSuccess": true,
          "Message": "OK"
        }
        ```

</details>

## Real Scenarios

Let say we have

```csharp
public record SingUpRequest(string Email, string Password);

public record SingUpResponse(Guid Id);

public static class Errors
{
    public static class Users
    {
        public static Error EmailAlreadyExists = "Email is already used";
    }
}
```

DI registration

```csharp
bulider.Services.AddOperationSerializerOptions();
```

Logic

```csharp
public class SingUpRequestHandler : IRequestHandler<SingUpRequest, Operation<SingUpResponse>>
{
    private readonly IUserService _userService;

    public SingUpRequestHandler(IuserService userService)
    {
        _userService = userService;
    }

    public async Task<Operation<SingUpResponse>> HandelAsync(SingUpRequest request) // #1
        => await Operation<SingUpResponse>
            .FailureIf(await _userService.IsEmailExistsAsync(request.Email), Errors.Users.EmailAlreadyExists)
            // .FailureIf(await userService.IsEmailExistsAsync(request.Email), new Error("Email is already used")) #2
            // .FailureIf(await userService.IsEmailExistsAsync(request.Email), "Email is already used") #3
            // .FailureIf(await userService.IsEmailExistsAsync(request.Email), new SpecificError("Email","already used")) #4
            // .FailureIf(await userService.IsEmailExistsAsync(request.Email), op => op.Error(Errors.Users.EmailAlreadyExists)) #5 and more
            .OnSuccessAsync(async op =>
            {
                var user = new User(request.Email);
                await _userService.CraeteAsync(request.Email, request.Password);
                op.SetResponse(new SingUpResponse(user.Id));
            });

    public async Task<Operation<SingUpResponse>> HandelAsync(SingUpRequest request) // #2
    {
        if (await _userService.IsEmailExistsAsync(request.Email))
        {
            return Errors.Users.EmailAlreadyExists; // #1 implicit conversion
            // return Operation<SingUpResponse>.BadRequest().Error(Errors.Users.EmailAlreadyExists); #2
        }

        var user = new User(request.Email);
        await _userService.CraeteAsync(request.Email, request.Password);
        return new SingUpResponse(user.Id); // #1 implicit conversion
        // return Operation<SingUpResponse>.Ok().SetResponse(new SingUpResponse(user.Id)); #2
    }
}
```

Controllers APIs

```csharp
public class UsersController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> SingUp([FromBody] SingUpRequest request, [FromServices] IRequestHandler<SingUpRequest, Operation<SingUpResponse>> handler)
        => await handler.HandelAsync(request).ToIActionResultAsync();
}
```

Minimal APIs

```csharp
app.MapPost("/users/singUp", async ([FromBody] SingUpRequest request, [FromServices] IRequestHandler<SingUpRequest, Operation<SingUpResponse>> handler)
    => await handler.HandelAsync(request).ToIResultAsync());
```

Outputs

```json
{
  "IsSuccess": true,
  "Message": "Ok",
  "Response": {
    "Id": "7BA82D5E-5A84-463B-80A1-34D923F9B027"
  }
}
```

```json
{
  "IsSuccess": false,
  "Message": "BadRequest",
  "Errors": [
    "Email is already used"
  ]
}
```
