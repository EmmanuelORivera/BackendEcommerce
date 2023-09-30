using Newtonsoft.Json;

namespace Ecommerce.Api.Errors;

public class CodeErrorResponse
{
    [JsonProperty(PropertyName = "statusCode")]
    public int StatusCode { get; set; }
    [JsonProperty(PropertyName = "message")]
    public string[]? Message { get; set; }

    public CodeErrorResponse(int statusCode, string[]? message = null)
    {
        StatusCode = statusCode;
        if (message is null)
        {
            Message = new string[0];
            var text = GetDefaultMessageStatusCode(statusCode);
            Message[0] = text;
        }
        else
        {
            Message = message;
        }
    }

    private string GetDefaultMessageStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "The Request Has Errors",
            401 => "No Authorizated",
            404 => "Not Found",
            500 => "Server Error",
            _ => string.Empty
        };
    }
}