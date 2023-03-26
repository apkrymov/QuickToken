using System.Text.Json.Serialization;
using MediatR;
using QuickToken.Core.Shared.Contracts.Responses;

namespace QuickToken.Core.Shared.Contracts.Requests;

public class MintCurrencyRequest : BlockchainRequest, IRequest<HandleResult>
{
    [JsonPropertyName("amount")]
    public ulong Amount { get; set; }
    
    [JsonPropertyName("address")]
    public string? Address { get; set; }
}