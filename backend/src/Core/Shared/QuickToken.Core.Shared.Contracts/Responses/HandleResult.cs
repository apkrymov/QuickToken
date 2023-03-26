namespace QuickToken.Core.Shared.Contracts.Responses;

public class HandleResult
{
    public bool IsComplete { get; set; }

    public string? Hash { get; set; }

    public BlockchainResponse? Response { get; set; }
}