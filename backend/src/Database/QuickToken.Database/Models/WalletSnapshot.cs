namespace QuickToken.Database.Models;

public class WalletSnapshot
{
    public Guid Id { get; set; }
    
    public DateTimeOffset Timestamp { get; set; }

    // TODO: bigint converter
    public string? Eth { get; set; }
    
    // TODO: bigint converter
    public string? Currency { get; set; }

    public Guid? WalletId { get; set; }

    public Wallet Wallet { get; set; }
}