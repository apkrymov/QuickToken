using System.Text.Json.Serialization;
using MediatR;
using QuickToken.Core.Shared.Contracts.Responses;

namespace QuickToken.Core.Shared.Contracts.Requests;

public class MintAssetsSerialRequest : BlockchainRequest, IRequest<HandleResult>
{
    [JsonPropertyName("id")] 
    public Guid Id { get; set; }
    
    [JsonPropertyName("token_ids")] 
    public Guid[] TokenIds { get; set; }
    
    [JsonPropertyName("ipo_price")] 
    public ulong IpoPrice { get; set; }

    [JsonPropertyName("daily_interest_rate")] 
    public double DailyInterestRate { get; set; }
    
    [JsonPropertyName("ipo_timestamp")] 
    public ulong IpoTimestamp { get; set; }

    [JsonPropertyName("burn_timestamp")] 
    public ulong BurnTimestamp { get; set; }
}