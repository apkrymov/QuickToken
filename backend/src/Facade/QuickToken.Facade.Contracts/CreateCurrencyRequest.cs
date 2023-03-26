using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickToken.Facade.Contracts;

public class CreateCurrencyRequest
{
    [JsonPropertyName("amount")]
    [Range(1,int.MaxValue)]
    public int Amount { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }
}