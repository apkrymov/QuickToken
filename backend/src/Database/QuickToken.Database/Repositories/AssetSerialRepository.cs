using Autofac.Features.OwnedInstances;
using Microsoft.EntityFrameworkCore;
using QuickToken.Database.Models;
using QuickToken.Shared.Converters;

namespace QuickToken.Database.Repositories; 

public class AssetSerialRepository : IAssetSerialRepository
{
    private readonly Func<Owned<DatabaseContext>> _dbFactory;

    public AssetSerialRepository(Func<Owned<DatabaseContext>> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<AssetSerial> AddAsync(Guid[] tokenIds, int price, double dailyInterestRate, TimeSpan duration, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var ipoTimestamp = DateTimeOffset.Now;
        
        var assets = tokenIds.Select(tokenId => new Asset
        {
            TokenId = tokenId
        });
        
        var result = await db.AssetSerials.AddAsync(new AssetSerial
        {
            Price = price,
            DailyInterestRate = dailyInterestRate,
            IpoTimestamp = ipoTimestamp,
            BurnTimestamp = ipoTimestamp + duration,
            Assets = assets.ToList()
        }, ct);
        
        await db.SaveChangesAsync(ct);
        return result.Entity;
    }

    public async Task<AssetSerial> FindByIdAsync(Guid id, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        return await db.AssetSerials
            .Include(p => p.Assets)
            .ThenInclude(p => p.Wallet)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<AssetSerial[]> FindByAddressAsync(string address, Paging paging, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        return await db.AssetSerials
            .Include(p => p.Assets)
            .ThenInclude(p => p.Wallet)
            .Where(p => p.Assets.Any(asset => asset.Wallet != null && asset.Wallet.Address == address))
            .Skip(paging.Shift)
            .Take(paging.Count)
            .AsNoTracking()
            .ToArrayAsync(ct);
    }

    public async Task<AssetSerial[]> FindAsync(Paging paging, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        return await db.AssetSerials
            .OrderBy(p => p.Id)
            .Skip(paging.Shift)
            .Take(paging.Count)
            .AsNoTracking()
            .ToArrayAsync(ct);
    }
}