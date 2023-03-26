using Microsoft.Extensions.Logging;
using QuickToken.Core.EthAdapter.Domain.Rpc;

namespace QuickToken.Core.EthAdapter.Domain.Handlers;

public class AbstractHandler
{
    protected readonly ILogger<AbstractHandler> Logger;
    protected readonly IWeb3Factory Web3Factory;

    public AbstractHandler(ILogger<AbstractHandler> logger, IWeb3Factory web3Factory)
    {
        Logger = logger;
        Web3Factory = web3Factory;
    }
}