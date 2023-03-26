using Microsoft.Extensions.Logging;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using QuickToken.Core.EthAdapter.Domain.Options;

namespace QuickToken.Core.EthAdapter.Domain.Rpc;

public class Web3Factory : IWeb3Factory
{
    private readonly ILogger<Web3Factory> _logger;

    private readonly AccountsByRoleOptions _accounts;
    private readonly Web3GateOptions _gateOptions;

    public Web3Factory(ILogger<Web3Factory> logger, AccountsByRoleOptions accounts, Web3GateOptions gateOptions)
    {
        _accounts = accounts;
        _gateOptions = gateOptions;
        _logger = logger;
    }

    public Web3 Create()
    {
        var web3Client = new RpcClient(baseUrl: _gateOptions.GetUri(), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        return new Web3(web3Client);
    }

    public Web3 CreateFromRole(Role role)
    {
        var web3Account = new Account(_accounts[role.ToString()]);
        var web3Client = new RpcClient(baseUrl: _gateOptions.GetUri(), authHeaderValue: null,
            jsonSerializerSettings: null,
            httpClientHandler: null, log: _logger);
        return new Web3(web3Account, web3Client);
    }
}