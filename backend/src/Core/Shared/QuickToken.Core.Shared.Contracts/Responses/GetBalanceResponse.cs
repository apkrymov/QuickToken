using System.Text.Json.Serialization;

namespace QuickToken.Core.Shared.Contracts.Responses;

public class GetBalanceResponse : BlockchainResponse
{
    [JsonPropertyName("eth")]
    public string Eth { get; set; }
    
    [JsonPropertyName("currency")]
    public string Currency { get; set; }
    
    [JsonPropertyName("assets_token_ids")]
    public Guid[] AssetTokenIds { get; set; }
}