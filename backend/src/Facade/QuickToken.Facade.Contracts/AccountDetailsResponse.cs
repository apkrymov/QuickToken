using System.Text.Json.Serialization;

namespace QuickToken.Facade.Contracts;

public class AccountDetailsResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("roles")]
    public string[] Roles { get; set; }
}