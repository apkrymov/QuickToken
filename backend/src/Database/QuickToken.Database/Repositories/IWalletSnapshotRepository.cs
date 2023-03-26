using QuickToken.Database.Models;

namespace QuickToken.Database.Repositories; 

public interface IWalletSnapshotRepository
{
    public Task<WalletSnapshot> AddAsync(Guid walletId, string eth, string currency, CancellationToken ct);
    
    public Task<WalletSnapshot[]> GetAsync(Guid walletId, Paging paging, CancellationToken ct);
}