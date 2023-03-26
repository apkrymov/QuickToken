namespace QuickToken.Facade.Domain.Models;

public class AssetSerialProposal
{
    public int Supply { get; set; }
    
    public int Volume { get; set; }
    
    public double DailyInterestRate { get; set; }

    public TimeSpan Duration { get; set; }
}