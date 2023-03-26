using System.Text.Json.Serialization;

namespace QuickToken.Facade.Contracts;

public class AssetSerialResponse
{
    [JsonPropertyName("id")] 
    public Guid Id { get; set; }

    [JsonPropertyName("price")] 
    public long Price { get; set; }
    
    [JsonPropertyName("daily_interest_rate")] 
    public double DailyInterestRate { get; set; }

    [JsonPropertyName("burn_timestamp")] 
    public DateTimeOffset BurnTimestamp { get; set; }
}