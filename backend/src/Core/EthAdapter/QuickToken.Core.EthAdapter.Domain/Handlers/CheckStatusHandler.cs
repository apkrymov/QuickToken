using MediatR;
using Microsoft.Extensions.Logging;
using Nethereum.RPC.Eth.DTOs;
using QuickToken.Core.EthAdapter.Domain.Internal;
using QuickToken.Core.EthAdapter.Domain.Rpc;
using QuickToken.Core.Shared.Contracts.Responses;

namespace QuickToken.Core.EthAdapter.Domain.Handlers;

// ReSharper disable once UnusedType.Global
public class CheckStatusHandler : AbstractHandler, IRequestHandler<CheckStatusRequest, HandleResult>
{
    public CheckStatusHandler(ILogger<CheckStatusHandler> logger, IWeb3Factory web3Factory) : base(logger, web3Factory)
    {
    }

    public async Task<HandleResult> Handle(CheckStatusRequest request, CancellationToken cancellationToken)
    {
        var web3 = Web3Factory.Create();
        var result = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(request.Hash);

        if (result is null)
        {
            return new HandleResult
            {
                IsComplete = false
            };
        }
        
        // TODO: more specified failure reason
        if (result.Failed())
        {
            throw new Exception("Transaction failed");
        }

        return new HandleResult
        {
            IsComplete = result.Succeeded()
        };
    }
}