using System.Text.Json.Serialization;

namespace QuickToken.Facade.Contracts;

public class AssetResponse
{
    [JsonPropertyName("token_id")]
    public Guid TokenId { get; set; }
    
    [JsonPropertyName("serial_id")]
    public Guid SerialId { get; set; }
}