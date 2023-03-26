using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickToken.Facade.Contracts;

public class TransferCurrencyRequest
{
    [JsonPropertyName("address")]
    public string Address { get; set; }
    
    [Range(1,int.MaxValue)]
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
}