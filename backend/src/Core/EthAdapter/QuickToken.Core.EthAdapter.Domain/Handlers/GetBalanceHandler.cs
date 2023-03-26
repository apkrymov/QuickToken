using System.Numerics;
using MediatR;
using Microsoft.Extensions.Logging;
using Nethereum.Hex.HexConvertors.Extensions;
using QuickToken.Core.Shared.Contracts.Requests;
using QuickToken.Core.Shared.Contracts.Responses;
using QuickToken.Core.EthAdapter.Domain.Rpc;
using QuickToken.Shared.Converters;
using QuickToken.Shared.Eth;

namespace QuickToken.Core.EthAdapter.Domain.Handlers;

// ReSharper disable once UnusedType.Global
public class GetBalanceHandler : AbstractHandler, IRequestHandler<GetBalanceRequest, HandleResult>
{
    public GetBalanceHandler(ILogger<GetBalanceHandler> logger, IWeb3Factory web3Factory) : base(logger, web3Factory)
    {
    }

    public async Task<HandleResult> Handle(GetBalanceRequest request, CancellationToken cancellationToken)
    {
        var web3 = Web3Factory.Create();
        var qtkcService = web3.Eth.ERC20.GetContractService(QtkcContract.Address);
        var qtkaService = web3.Eth.ERC721.GetContractService(QtkaContract.Address);

        var ethBalance = await web3.Eth.GetBalance.SendRequestAsync(request.Address);
        var qtkcBalance = await qtkcService.BalanceOfQueryAsync(request.Address);
        var qtkaIds =
            await qtkaService.GetAllTokenIdsOfOwnerUsingTokenOfOwnerByIndexAndMultiCallAsync(request.Address);

        return new HandleResult
        {
            IsComplete = true,
            Response = new GetBalanceResponse
            {
                Eth = ethBalance.ToString(),
                Currency = qtkcBalance.ToString(),
                AssetTokenIds = qtkaIds.Select(p => p.Create()).ToArray()
            }
        };
    }
}