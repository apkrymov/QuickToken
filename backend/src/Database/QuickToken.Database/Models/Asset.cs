namespace QuickToken.Database.Models;

public class Asset
{
    public Guid Id { get; set; }
    
    public Guid TokenId { get; set; }
    
    public Guid? WalletId { get; set; }

    public Wallet? Wallet { get; set; }
    
    public Guid AssetSerialId { get; set; }

    public AssetSerial AssetSerial { get; set; }
}