using MediatR;
using QuickToken.Core.Shared.Contracts.Responses;
using QuickToken.Core.Shared.Contracts.Tools;
using QuickToken.Database.Models;
using QuickToken.Database.Repositories;
using QuickToken.Shared.Worker;
using QuickToken.Shared.Worker.Options;

namespace QuickToken.Core.EthAdapter.Workers;

// ReSharper disable once ClassNeverInstantiated.Global
public class HandleWorker : AbstractWorker<BlockchainTransaction>
{
    private readonly IBlockchainTransactionRepository _transactions;
    private readonly IMediator _mediator;

    public HandleWorker(ILogger<HandleWorker> logger, BaseWorkerOptions options,
        IBlockchainTransactionRepository transactions, IMediator mediator) : base(logger, options)
    {
        _transactions = transactions;
        _mediator = mediator;
    }

    protected override async Task<BlockchainTransaction[]> GetWorkAsync(Paging paging, CancellationToken ct)
    {
        return await _transactions.GetNewAsync(paging, ct);
    }

    protected override async Task ProcessWorkAsync(BlockchainTransaction transaction, CancellationToken ct)
    {
        Logger.LogInformation("Processing new transaction {TransactionId}", transaction.Id);
        var input = Payload.DeserializeRequest(transaction.InputPayload);

        HandleResult? output;
        try
        {
            output = await _mediator.Send(input, ct) as HandleResult;
        }
        catch (Exception ex)
        {
            var error = new ErrorResponse
            {
                Message = ex.Message,
                Stacktrace = ex.StackTrace
            };
            await _transactions.MarkFailed(transaction.Id, Payload.SerializeResponse(error), ct);
            Logger.LogError(ex, "Transaction {TransactionId} failed", transaction.Id);
            return;
        }

        if (output.IsComplete)
        {
            await _transactions.MarkSucceeded(transaction.Id, Payload.SerializeResponse(output.Response), ct);
            Logger.LogInformation("Transaction {TransactionId} succeeded", transaction.Id);
        }
        else
        {
            await _transactions.MarkInProgress(transaction.Id, output.Hash, ct);
            Logger.LogInformation("Transaction {TransactionId} pushed to network with hash {TransactionHash}", transaction.Id, output.Hash);
        }
    }
}