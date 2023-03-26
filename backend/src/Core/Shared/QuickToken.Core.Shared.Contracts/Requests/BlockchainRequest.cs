using System.Text.Json.Serialization;

namespace QuickToken.Core.Shared.Contracts.Requests;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(GetBalanceRequest), nameof(GetBalanceRequest))]
[JsonDerivedType(typeof(MintAssetsSerialRequest), nameof(MintAssetsSerialRequest))]
[JsonDerivedType(typeof(MintCurrencyRequest), nameof(MintCurrencyRequest))]
public abstract class BlockchainRequest
{
}