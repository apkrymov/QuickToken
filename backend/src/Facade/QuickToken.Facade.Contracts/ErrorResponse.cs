using System.Text.Json.Serialization;

namespace QuickToken.Facade.Contracts;

public class ErrorResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
    
    [JsonPropertyName("details")]
    public string Details { get; set; }
}