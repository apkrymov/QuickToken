using Autofac.Features.OwnedInstances;
using Microsoft.EntityFrameworkCore;
using QuickToken.Database.Models;

namespace QuickToken.Database.Repositories;

public class WalletSnapshotRepository : IWalletSnapshotRepository
{
    private readonly Func<Owned<DatabaseContext>> _dbFactory;

    public WalletSnapshotRepository(Func<Owned<DatabaseContext>> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<WalletSnapshot> AddAsync(Guid walletId, string eth, string currency, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var walletSnapshot = new WalletSnapshot
        {
            Eth = eth,
            Currency = currency,
            WalletId = walletId,
            Timestamp = DateTimeOffset.Now
        };
        var lastWalletSnapshot = db.WalletSnapshots.OrderByDescending(p => p.Timestamp).FirstOrDefault();
        if (lastWalletSnapshot?.Currency != walletSnapshot.Currency ||
            lastWalletSnapshot?.Eth != walletSnapshot.Eth)
        {
            await db.WalletSnapshots.AddAsync(walletSnapshot, ct);
            await db.SaveChangesAsync(ct);
        }

        return walletSnapshot;
    }

    public async Task<WalletSnapshot[]> GetAsync(Guid walletId, Paging paging, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        return await db.WalletSnapshots
            .Where(p => p.WalletId == walletId)
            .OrderBy(p => p.Timestamp)
            .Skip(paging.Shift)
            .Take(paging.Count)
            .AsNoTracking()
            .ToArrayAsync(ct);
    }
}