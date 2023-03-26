using Nethereum.Web3;
using QuickToken.Core.EthAdapter.Domain.Options;

namespace QuickToken.Core.EthAdapter.Domain.Rpc;

public interface IWeb3Factory
{
    public Web3 Create();
    
    public Web3 CreateFromRole(Role role);
}