using QuickToken.Core.Cache.Options;
using QuickToken.Core.Shared.Contracts.Responses;
using QuickToken.Core.Shared.Services;
using QuickToken.Core.Shared.Services.Polling;
using QuickToken.Database.Models;
using QuickToken.Database.Repositories;
using QuickToken.Shared.Worker;

namespace QuickToken.Core.Cache.Workers;

// ReSharper disable once ClassNeverInstantiated.Global
public class WalletWorker : AbstractWorker<Wallet>
{
    private readonly IWalletRepository _wallets;
    private readonly IWalletSnapshotRepository _walletSnapshots;
    private readonly IAssetRepository _assets;
    private readonly CacheWorkerOptions _options;

    private readonly IBlockchainService _blockchain;
    private readonly ITransactionPollingService _pollingService;

    public WalletWorker(ILogger<WalletWorker> logger, CacheWorkerOptions options, IWalletRepository wallets,
        IBlockchainService blockchain, ITransactionPollingService pollingService, IAssetRepository assets,
        IWalletSnapshotRepository walletSnapshots) :
        base(logger, options)
    {
        _options = options;
        _wallets = wallets;
        _blockchain = blockchain;
        _pollingService = pollingService;
        _assets = assets;
        _walletSnapshots = walletSnapshots;
    }

    protected override async Task ProcessWorkAsync(Wallet wallet, CancellationToken ct)
    {
        try
        {
            await ProcessWorkInternalAsync(wallet, ct);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occured during cache update. Cache counters will be reset to prevent spam");
        }
        finally
        {
            await ResetCacheUpdate(wallet, ct);
        }
    }

    protected override async Task<Wallet[]> GetWorkAsync(Paging paging, CancellationToken ct)
    {
        return await _wallets.GetExpiredCacheAsync(_options.CacheExpirationPeriod, paging, ct);
    }

    private async Task ResetCacheUpdate(Wallet wallet, CancellationToken ct)
    {
        await _wallets.ResetCacheUpdateAsync(wallet.Id, ct);
    }

    private async Task ProcessWorkInternalAsync(Wallet wallet, CancellationToken ct)
    {
        Logger.LogInformation("Processing expired wallet {WalletId}", wallet.Id);

        var transactionId = await _blockchain.GetBalanceAsync(wallet.Address, ct);
        var result = await _pollingService.PollAsync<GetBalanceResponse>(transactionId, ct);

        await _wallets.SetBalanceAsync(wallet.Id, eth: result.Eth, currency: result.Currency, ct);
        Logger.LogInformation("Currency balance in wallet {WalletId} updated", wallet.Id);

        await _assets.BatchSyncAsync(result.AssetTokenIds, wallet.Id, ct);
        Logger.LogInformation("Ownership of assets in wallet {WalletId} updated", wallet.Id);
        
        await _walletSnapshots.AddAsync(wallet.Id, eth: result.Eth, currency: result.Currency, ct);
        Logger.LogInformation("Snapshot of wallet {WalletId} added", wallet.Id);
    }
}