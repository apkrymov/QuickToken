using MediatR;
using Microsoft.Extensions.Logging;
using QuickToken.Core.EthAdapter.Contracts.QTKC;
using QuickToken.Core.EthAdapter.Domain.Options;
using QuickToken.Core.Shared.Contracts.Requests;
using QuickToken.Core.EthAdapter.Domain.Rpc;
using QuickToken.Core.Shared.Contracts.Responses;
using QuickToken.Shared.Eth;

namespace QuickToken.Core.EthAdapter.Domain.Handlers;

// ReSharper disable once UnusedType.Global
public class MintCurrencyHandler : AbstractHandler, IRequestHandler<MintCurrencyRequest, HandleResult>
{
    public MintCurrencyHandler(ILogger<MintCurrencyHandler> logger, IWeb3Factory web3Factory) : base(logger, web3Factory)
    {
    }

    public async Task<HandleResult> Handle(MintCurrencyRequest request, CancellationToken cancellationToken)
    {
        var web3 = Web3Factory.CreateFromRole(Role.Owner);
        
        var contractHandler = web3.Eth.GetContractTransactionHandler<QtkcMintFunction>();
        var contractMessage = new QtkcMintFunction
        {
            To = request.Address ?? DexContract.Address,
            Amount = request.Amount
        };

        var hash = await contractHandler.SendRequestAsync(QtkcContract.Address, contractMessage);
        return new HandleResult
        {
            Hash = hash,
            IsComplete = false
        };
    }
}