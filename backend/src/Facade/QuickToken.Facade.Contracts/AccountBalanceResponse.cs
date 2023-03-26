using System.Text.Json.Serialization;

namespace QuickToken.Facade.Contracts;

public class AccountBalanceResponse
{
    [JsonPropertyName("eth")] 
    public string Eth { get; set; }

    [JsonPropertyName("currency")] 
    public string Currency { get; set; }

    [JsonPropertyName("assets")] 
    public AssetResponse[] Assets { get; set; }
}