using System.Text.Json.Serialization;

namespace QuickToken.Facade.Contracts;

public class DexAssetSerialResponse : AssetSerialResponse
{
    [JsonPropertyName("in_stock")] 
    public int InStock { get; set; }
}