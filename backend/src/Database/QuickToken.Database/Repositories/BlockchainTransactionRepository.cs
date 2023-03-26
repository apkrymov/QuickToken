using Autofac.Features.OwnedInstances;
using Microsoft.EntityFrameworkCore;
using QuickToken.Database.Models;

namespace QuickToken.Database.Repositories;

public class BlockchainTransactionRepository : IBlockchainTransactionRepository
{
    private readonly Func<Owned<DatabaseContext>> _dbFactory;

    public BlockchainTransactionRepository(Func<Owned<DatabaseContext>> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<BlockchainTransaction> CreateAsync(string input, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var transaction = new BlockchainTransaction
        {
            State = State.New,
            InputPayload = input,
            CreatedAt = DateTimeOffset.Now,
            LastUpdateAt = DateTimeOffset.Now
        };
        var result = await db.BlockchainTransactions.AddAsync(transaction, ct);
        await db.SaveChangesAsync(ct);
        return result.Entity;
    }

    public async Task<BlockchainTransaction> MarkInProgress(Guid id, string hash, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var transaction = await db.BlockchainTransactions.SingleAsync(p => p.Id == id, ct);
        transaction.State = State.InProgress;
        transaction.Hash = hash;
        transaction.LastUpdateAt = DateTimeOffset.Now;
        await db.SaveChangesAsync(ct);
        return transaction;
    }

    public async Task<BlockchainTransaction> MarkSucceeded(Guid id, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var transaction = await db.BlockchainTransactions.SingleAsync(p => p.Id == id, ct);
        transaction.State = State.Succeeded;
        transaction.LastUpdateAt = DateTimeOffset.Now;
        await db.SaveChangesAsync(ct);
        return transaction;
    }

    public async Task<BlockchainTransaction> MarkFailed(Guid id, string output, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var transaction = await db.BlockchainTransactions.SingleAsync(p => p.Id == id, ct);
        transaction.State = State.Failed;
        transaction.OutputPayload = output;
        transaction.LastUpdateAt = DateTimeOffset.Now;
        await db.SaveChangesAsync(ct);
        return transaction;
    }

    public async Task<BlockchainTransaction> MarkSucceeded(Guid id, string output, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var transaction = await db.BlockchainTransactions.SingleAsync(p => p.Id == id, ct);
        transaction.State = State.Succeeded;
        transaction.OutputPayload = output;
        transaction.LastUpdateAt = DateTimeOffset.Now;
        await db.SaveChangesAsync(ct);
        return transaction;
    }

    public async Task<BlockchainTransaction[]> GetNewAsync(Paging paging, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        return await db.BlockchainTransactions
            .Where(p => p.State == State.New)
            .OrderBy(p => p.CreatedAt)
            .Skip(paging.Shift)
            .Take(paging.Count)
            .AsNoTracking()
            .ToArrayAsync(ct);
    }

    public async Task<BlockchainTransaction> FindAsync(Guid id, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        return await db.BlockchainTransactions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
    }
    
    public async Task<BlockchainTransaction[]> GetInProgressAsync(Paging paging, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        return await db.BlockchainTransactions
            .Where(p => p.State == State.InProgress)
            .OrderBy(p => p.CreatedAt)
            .Skip(paging.Shift)
            .Take(paging.Count)
            .AsNoTracking()
            .ToArrayAsync(ct);
    }
}