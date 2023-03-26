using System.Text.Json.Serialization;

namespace QuickToken.Core.Shared.Contracts.Responses;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ErrorResponse), nameof(ErrorResponse))]
[JsonDerivedType(typeof(GetBalanceResponse), nameof(GetBalanceResponse))]
public abstract class BlockchainResponse
{
}