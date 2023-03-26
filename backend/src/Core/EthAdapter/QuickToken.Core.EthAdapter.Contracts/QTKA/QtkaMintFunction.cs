using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace QuickToken.Core.EthAdapter.Contracts.QTKA;

[Function("serialMint")]
public class QtkaMintFunction : FunctionMessage
{
    [Parameter("address", "to", 1)]
    public string To { get; set; }
    
    [Parameter("uint256[]", "tokenIds", 2)]
    public BigInteger[] TokenIds { get; set; }
    
    [Parameter("tuple", "inputMetadata", 3)]
    public QtkaMetadataStruct Metadata { get; set; }
}