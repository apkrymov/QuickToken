namespace QuickToken.Core.EthAdapter.Domain.Options;

public class Web3GateOptions
{
    public Web3GateProvider Provider { get; set; }

    public string ApiKey { get; set; }

    public string Network { get; set; }

    public Uri GetUri()
    {
        return Provider switch
        {
            Web3GateProvider.Infura => new Uri($"https://{Network}.infura.io/v3/{ApiKey}"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}