namespace QuickToken.Database.Models;

public class AssetSerial
{
    public Guid Id { get; set; }
    
    public long Price { get; set; }

    public double DailyInterestRate { get; set; }

    public DateTimeOffset IpoTimestamp { get; set; }
    
    public DateTimeOffset BurnTimestamp { get; set; }

    public List<Asset> Assets { get; set; }
}