using System.Numerics;
using MediatR;
using Microsoft.Extensions.Logging;
using QuickToken.Core.EthAdapter.Contracts.QTKA;
using QuickToken.Core.EthAdapter.Domain.Options;
using QuickToken.Core.Shared.Contracts.Requests;
using QuickToken.Core.EthAdapter.Domain.Rpc;
using QuickToken.Core.Shared.Contracts.Responses;
using QuickToken.Shared.Converters;
using QuickToken.Shared.Eth;

namespace QuickToken.Core.EthAdapter.Domain.Handlers;

// ReSharper disable once UnusedType.Global
public class MintAssetsSerialHandler : AbstractHandler, IRequestHandler<MintAssetsSerialRequest, HandleResult>
{
    public MintAssetsSerialHandler(ILogger<MintAssetsSerialHandler> logger, IWeb3Factory web3Factory) : base(logger, web3Factory)
    {
    }

    public async Task<HandleResult> Handle(MintAssetsSerialRequest request, CancellationToken cancellationToken)
    {
        var web3 = Web3Factory.CreateFromRole(Role.Owner);

        var contractHandler = web3.Eth.GetContractTransactionHandler<QtkaMintFunction>();
        var dailyInterestRate = new BigInteger(request.DailyInterestRate * Math.Pow(10, QtkaContract.InterestRateDecimals));

        var contractMessage = new QtkaMintFunction
        {
            To = DexContract.Address,
            TokenIds = request.TokenIds.Select(tokenId => tokenId.ToBigInteger()).ToArray(),
            Metadata = new QtkaMetadataStruct
            {
                IpoSerial = request.Id.ToBigInteger(),
                IpoPrice = request.IpoPrice,
                DailyInterestRate = dailyInterestRate,
                IpoTimestamp = request.IpoTimestamp,
                BurnTimestamp = request.BurnTimestamp
            }
        };

        var hash = await contractHandler.SendRequestAsync(QtkaContract.Address, contractMessage);
        return new HandleResult
        {
            Hash = hash,
            IsComplete = false
        };
    }
}