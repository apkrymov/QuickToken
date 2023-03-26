using System.Data;
using Autofac.Features.OwnedInstances;
using Microsoft.EntityFrameworkCore;

namespace QuickToken.Database.Repositories;

public class AssetRepository : IAssetRepository
{
    private readonly Func<Owned<DatabaseContext>> _dbFactory;

    public AssetRepository(Func<Owned<DatabaseContext>> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task BatchSyncAsync(Guid[] tokenIds, Guid walletId, CancellationToken ct)
    {
        var db = _dbFactory().Value;

        // Lock operations to single transaction.
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, ct);

        // Unlink all assets
        await db.Assets
            .Where(u => u.WalletId == walletId)
            .ExecuteUpdateAsync(b =>
                b.SetProperty(u => u.WalletId, (Guid?) null), ct);
        
        // Link actual assets
        await db.Assets
            .Where(u => tokenIds.Contains(u.TokenId))
            .ExecuteUpdateAsync(b =>
                b.SetProperty(u => u.WalletId, walletId), ct);
        
        await transaction.CommitAsync(ct);
    }
}