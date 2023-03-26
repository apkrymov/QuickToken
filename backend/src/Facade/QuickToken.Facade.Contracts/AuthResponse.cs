using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickToken.Facade.Contracts;

public class AuthResponse
{
    [JsonPropertyName("access_token")]
    [Required]
    public string AccessToken { get; set; }
}