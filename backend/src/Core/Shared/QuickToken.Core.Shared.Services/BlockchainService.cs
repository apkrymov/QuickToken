using Microsoft.Extensions.Logging;
using QuickToken.Core.Shared.Contracts.Requests;
using QuickToken.Core.Shared.Contracts.Responses;
using QuickToken.Core.Shared.Contracts.Tools;
using QuickToken.Core.Shared.Services.Exceptions;
using QuickToken.Database.Models;
using QuickToken.Database.Repositories;

namespace QuickToken.Core.Shared.Services;

public class BlockchainService : IBlockchainService
{
    private readonly ILogger<BlockchainService> _logger;
    private readonly IBlockchainTransactionRepository _transactions;

    public BlockchainService(ILogger<BlockchainService> logger,
        IBlockchainTransactionRepository transactions)
    {
        _transactions = transactions;
        _logger = logger;
    }

    public async Task<Guid> MintCurrencyAsync(long amount, CancellationToken ct)
    {
        return await MintCurrencyAsync(amount, null, ct);
    }

    public async Task<Guid> MintCurrencyAsync(long amount, string? address, CancellationToken ct)
    {
        var request = new MintCurrencyRequest
        {
            Amount = (ulong) amount,
            Address = address
        };
        var transaction = await _transactions.CreateAsync(Payload.SerializeRequest(request), ct);
        return transaction.Id;
    }

    public async Task<Guid> MintAssetsSerialAsync(Guid id, Guid[] tokenIds, long price, double dailyInterestRate,
        DateTimeOffset ipoTimestamp, DateTimeOffset burnTimestamp, CancellationToken ct)
    {
        var request = new MintAssetsSerialRequest
        {
            Id = id,
            TokenIds = tokenIds,
            IpoPrice = (ulong) price,
            DailyInterestRate = dailyInterestRate,
            IpoTimestamp = (ulong) ipoTimestamp.ToUniversalTime().ToUnixTimeSeconds(),
            BurnTimestamp = (ulong) burnTimestamp.ToUniversalTime().ToUnixTimeSeconds()
        };
        var transaction = await _transactions.CreateAsync(Payload.SerializeRequest(request), ct);
        return transaction.Id;
    }

    public async Task<Guid> GetBalanceAsync(string address, CancellationToken ct)
    {
        var request = new GetBalanceRequest
        {
            Address = address
        };
        var transaction = await _transactions.CreateAsync(Payload.SerializeRequest(request), ct);
        return transaction.Id;
    }

    public async Task<(bool isCompleted, BlockchainResponse? payload)> GetTransactionAsync(Guid id,
        CancellationToken ct)
    {
        var transaction = await _transactions.FindAsync(id, ct);
        if (transaction == null)
            throw new TransactionNotFoundException($"Transaction {id} not found");
        switch (transaction.State)
        {
            case State.Succeeded:
                var payload = Payload.DeserializeResponse(transaction.OutputPayload);
                return (true, payload);
            case State.Failed:
                var error = Payload.DeserializeResponse(transaction.OutputPayload) as ErrorResponse;
                throw new TransactionFailedException($"Transaction {id} failed", error);
            default:
                return (false, null);
        }
    }
}