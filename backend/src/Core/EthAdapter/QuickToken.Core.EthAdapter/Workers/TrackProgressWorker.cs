using MediatR;
using QuickToken.Core.EthAdapter.Domain.Internal;
using QuickToken.Database.Models;
using QuickToken.Database.Repositories;
using QuickToken.Shared.Worker;
using QuickToken.Shared.Worker.Options;

namespace QuickToken.Core.EthAdapter.Workers;

// ReSharper disable once ClassNeverInstantiated.Global
public class TrackProgressWorker : AbstractWorker<BlockchainTransaction>
{
    private readonly IBlockchainTransactionRepository _transactions;
    private readonly IMediator _mediator;

    public TrackProgressWorker(ILogger<TrackProgressWorker> logger, BaseWorkerOptions options,
        IBlockchainTransactionRepository transactions, IMediator mediator) : base(logger, options)
    {
        _transactions = transactions;
        _mediator = mediator;
    }

    protected override async Task<BlockchainTransaction[]> GetWorkAsync(Paging paging, CancellationToken ct)
    {
        return await _transactions.GetInProgressAsync(paging, ct);
    }

    protected override async Task ProcessWorkAsync(BlockchainTransaction transaction, CancellationToken ct)
    {
        var result = await _mediator.Send(new CheckStatusRequest
        {
            Hash = transaction.Hash
        }, ct);

        if (result.IsComplete)
        {
            await _transactions.MarkSucceeded(transaction.Id, ct);
            Logger.LogInformation("Transaction {TransactionId} succeeded", transaction.Id);
        }
    }
}