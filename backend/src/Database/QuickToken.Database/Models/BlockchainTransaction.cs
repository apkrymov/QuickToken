namespace QuickToken.Database.Models;

public class BlockchainTransaction
{
    public Guid Id { get; set; }

    public const int InputPayloadMaxLength = 5000;
    public string InputPayload { get; set; }
    
    public const int OutputPayloadMaxLength = 5000;
    public string? OutputPayload { get; set; }

    public State State { get; set; }

    public const int HashMaxLength = 100;
    public string? Hash { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset LastUpdateAt { get; set; }
}