using QuickToken.Database.Models;

namespace QuickToken.Database.Repositories;

public interface IBlockchainTransactionRepository
{
    public Task<BlockchainTransaction> CreateAsync(string input, CancellationToken ct);
    
    public Task<BlockchainTransaction> FindAsync(Guid id, CancellationToken ct);
    
    public Task<BlockchainTransaction> MarkInProgress(Guid id, string hash, CancellationToken ct);
    
    public Task<BlockchainTransaction> MarkSucceeded(Guid id, CancellationToken ct);
    
    public Task<BlockchainTransaction> MarkFailed(Guid id, string output, CancellationToken ct);
    
    public Task<BlockchainTransaction> MarkSucceeded(Guid id, string output, CancellationToken ct);
    
    public Task<BlockchainTransaction[]> GetNewAsync(Paging paging, CancellationToken ct);
    
    public Task<BlockchainTransaction[]> GetInProgressAsync(Paging paging, CancellationToken ct);
}