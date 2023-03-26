using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickToken.Facade.Contracts;

public class WalletAuthRequest
{
    [JsonPropertyName("wallet")]
    [Required]
    public string Wallet { get; set; }
}