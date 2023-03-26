using System.Text.Json.Serialization;

namespace QuickToken.Facade.Contracts;

public class AssetSerialOwnersResponse : AssetSerialResponse
{
    [JsonPropertyName("owners")] 
    public Dictionary<string,int> Owners { get; set; }
}