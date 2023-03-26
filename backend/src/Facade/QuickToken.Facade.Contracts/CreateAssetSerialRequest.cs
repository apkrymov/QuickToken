using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickToken.Facade.Contracts;

public class CreateAssetSerialRequest
{
    [JsonPropertyName("supply")] 
    [Range(1,int.MaxValue)]
    public int Supply { get; set; }
    
    [Range(1,int.MaxValue)]
    [JsonPropertyName("volume")] 
    public int Volume { get; set; }
    [Range(0,double.MaxValue)]
    [JsonPropertyName("daily_interest_rate")] 
    public double DailyInterestRate { get; set; }

    [JsonPropertyName("duration")] 
    public TimeSpan Duration { get; set; }
}