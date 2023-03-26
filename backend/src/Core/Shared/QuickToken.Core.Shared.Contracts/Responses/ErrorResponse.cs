using System.Text.Json.Serialization;

namespace QuickToken.Core.Shared.Contracts.Responses;

public class ErrorResponse : BlockchainResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
    
    [JsonPropertyName("stacktrace")]
    public string? Stacktrace { get; set; }
}