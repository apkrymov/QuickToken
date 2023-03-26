namespace QuickToken.Database.Repositories;

public interface IAssetRepository
{
    public Task BatchSyncAsync(Guid[] tokenIds, Guid walletId, CancellationToken ct);
}