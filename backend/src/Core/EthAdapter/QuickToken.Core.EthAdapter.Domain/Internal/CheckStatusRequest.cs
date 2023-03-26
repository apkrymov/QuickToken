using MediatR;
using QuickToken.Core.Shared.Contracts.Requests;
using QuickToken.Core.Shared.Contracts.Responses;

namespace QuickToken.Core.EthAdapter.Domain.Internal;

public class CheckStatusRequest : BlockchainRequest, IRequest<HandleResult>
{
    public string Hash { get; set; }
}