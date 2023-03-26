using QuickToken.Core.Shared.Contracts.Responses;

namespace QuickToken.Core.Shared.Services;

public interface IBlockchainService
{
    public Task<Guid> MintCurrencyAsync(long amount, CancellationToken ct);
    
    public Task<Guid> MintCurrencyAsync(long amount, string address, CancellationToken ct);

    public Task<Guid> MintAssetsSerialAsync(Guid id, Guid[] tokenIds, long price, double dailyInterestRate,
        DateTimeOffset ipoTimestamp, DateTimeOffset burnTimestamp, CancellationToken ct);

    public Task<Guid> GetBalanceAsync(string address, CancellationToken ct);

    public Task<(bool isCompleted, BlockchainResponse? payload)> GetTransactionAsync(Guid id, CancellationToken ct);
}