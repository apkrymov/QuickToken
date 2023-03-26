using Autofac.Features.OwnedInstances;
using Microsoft.EntityFrameworkCore;
using QuickToken.Database.Models;

namespace QuickToken.Database.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly Func<Owned<DatabaseContext>> _dbFactory;

    public WalletRepository(Func<Owned<DatabaseContext>> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<Wallet> SetCacheUpdateAsync(Guid id, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var wallet = await db.Wallets.SingleAsync(p => p.Id == id, ct);
        wallet.ForceCacheUpdate = true;
        await db.SaveChangesAsync(ct);
        return wallet;
    }

    public async Task<Wallet> ResetCacheUpdateAsync(Guid id, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var wallet = await db.Wallets.SingleAsync(p => p.Id == id, ct);
        wallet.LastUpdateAt = DateTimeOffset.Now;
        wallet.ForceCacheUpdate = false;
        await db.SaveChangesAsync(ct);
        return wallet;
    }

    public async Task<Wallet> SetBalanceAsync(Guid id, string eth, string currency, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var wallet = await db.Wallets.SingleAsync(p => p.Id == id, ct);
        wallet.Eth = eth;
        wallet.Currency = currency;
        await db.SaveChangesAsync(ct);
        return wallet;
    }

    public async Task<Wallet[]> GetExpiredCacheAsync(TimeSpan expirationPeriod, Paging paging, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var latUpdateBorder = DateTimeOffset.Now - expirationPeriod;
        return await db.Wallets
            .Where(p => p.ForceCacheUpdate || p.LastUpdateAt == null || p.LastUpdateAt < latUpdateBorder)
            .OrderBy(p => p.LastUpdateAt)
            .Skip(paging.Shift)
            .Take(paging.Count)
            .AsNoTracking()
            .ToArrayAsync(ct);
    }
}