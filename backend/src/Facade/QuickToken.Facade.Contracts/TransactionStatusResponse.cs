using System.Text.Json.Serialization;
using QuickToken.Core.Shared.Contracts.Responses;

namespace QuickToken.Facade.Contracts;

public class TransactionStatusResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("state")]
    public TransactionState State { get; set; }

    [JsonPropertyName("payload")]
    public BlockchainResponse? Payload { get; set; }
}