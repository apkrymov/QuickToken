using System.Text.Json.Serialization;

namespace QuickToken.Facade.Contracts;

public class BalanceSnapshotResponse
{
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; set; }
    
    [JsonPropertyName("eth")]
    public string? Eth { get; set; }
    
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }
}