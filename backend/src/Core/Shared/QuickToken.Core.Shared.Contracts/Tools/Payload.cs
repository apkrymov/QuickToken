using System.Text.Json;
using QuickToken.Core.Shared.Contracts.Requests;
using QuickToken.Core.Shared.Contracts.Responses;

namespace QuickToken.Core.Shared.Contracts.Tools;

public static class Payload
{
    public static string SerializeRequest(BlockchainRequest data)
    {
        return JsonSerializer.Serialize(data);
    }
    
    public static string SerializeResponse(BlockchainResponse data)
    {
        return JsonSerializer.Serialize(data);
    }
    
    public static BlockchainRequest DeserializeRequest(string data)
    {
        return JsonSerializer.Deserialize<BlockchainRequest>(data);
    }

    public static BlockchainResponse DeserializeResponse(string data)
    {
        // Response could be null, if no data back required
        return data is null ? null : JsonSerializer.Deserialize<BlockchainResponse>(data);
    }
}