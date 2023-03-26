using QuickToken.Database.Models;

namespace QuickToken.Database.Repositories; 

public interface IWalletRepository
{
    public Task<Wallet> SetCacheUpdateAsync(Guid id, CancellationToken ct);
    
    public Task<Wallet> ResetCacheUpdateAsync(Guid id, CancellationToken ct);
    
    public Task<Wallet> SetBalanceAsync(Guid id, string eth, string currency, CancellationToken ct);

    public Task<Wallet[]> GetExpiredCacheAsync(TimeSpan expirationPeriod, Paging paging, CancellationToken ct);
}