using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace QuickToken.Core.EthAdapter.Contracts.QTKA;

[Struct("QuickTokenAsset.Metadata")]
public class QtkaMetadataStruct
{
    [Parameter("uint256", "ipoSerial", 1)]
    public BigInteger IpoSerial { get; set; }
    
    [Parameter("uint256", "ipoPrice", 2)]
    public BigInteger IpoPrice { get; set; }
    
    [Parameter("uint256", "ipoTimestamp", 3)]
    public BigInteger IpoTimestamp { get; set; }
    
    [Parameter("uint256", "dailyInterestRate", 4)]
    public BigInteger DailyInterestRate { get; set; }
    
    [Parameter("uint256", "burnTimestamp", 5)]
    public BigInteger BurnTimestamp { get; set; }
}