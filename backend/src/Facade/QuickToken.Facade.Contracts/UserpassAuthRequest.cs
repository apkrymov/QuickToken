using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickToken.Facade.Contracts;

public class UserpassAuthRequest
{
    [JsonPropertyName("login")]
    [Required]
    public string Login { get; set; }
    
    [JsonPropertyName("password")]
    [Required]
    public string Password { get; set; }
}