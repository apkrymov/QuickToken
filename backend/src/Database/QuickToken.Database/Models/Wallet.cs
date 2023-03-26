namespace QuickToken.Database.Models;

public class Wallet
{
    public Guid Id { get; set; }
    
    public const int AddressMaxLength = 50;
    public string Address { get; set; }
    
    // TODO: bigint converter
    public string? Eth { get; set; }
    
    // TODO: bigint converter
    public string? Currency { get; set; }

    public Guid? AccountId { get; set; }
    
    public DateTimeOffset? LastUpdateAt { get; set; }
    
    public bool ForceCacheUpdate { get; set; }

    public List<Asset> Assets { get; set; }
    
    public List<WalletSnapshot> Snapshots { get; set; }
    
    public Account? Account { get; set; }
}