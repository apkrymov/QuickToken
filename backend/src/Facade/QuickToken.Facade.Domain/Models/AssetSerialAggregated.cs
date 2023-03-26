namespace QuickToken.Facade.Domain.Models;

public class AssetSerialAggregated
{
    public Guid Id { get; set; }
    
    public long Price { get; set; }

    public double DailyInterestRate { get; set; }

    public DateTimeOffset BurnTimestamp { get; set; }

    public Dictionary<string, int> Owners { get; set; }
}