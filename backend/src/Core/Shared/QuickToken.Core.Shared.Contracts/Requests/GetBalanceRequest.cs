using System.Text.Json.Serialization;
using MediatR;
using QuickToken.Core.Shared.Contracts.Responses;

namespace QuickToken.Core.Shared.Contracts.Requests;

public class GetBalanceRequest : BlockchainRequest, IRequest<HandleResult>
{
    [JsonPropertyName("address")]
    public string Address { get; set; }
}