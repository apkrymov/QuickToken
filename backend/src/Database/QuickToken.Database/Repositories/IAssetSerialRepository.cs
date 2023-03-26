using QuickToken.Database.Models;

namespace QuickToken.Database.Repositories; 

public interface IAssetSerialRepository
{
    public Task<AssetSerial> AddAsync(Guid[] tokenIds, int price, double dailyInterestRate, TimeSpan duration, CancellationToken ct);

    public Task<AssetSerial> FindByIdAsync(Guid id, CancellationToken ct);
    
    public Task<AssetSerial[]> FindByAddressAsync(string address, Paging paging, CancellationToken ct);
    
    public Task<AssetSerial[]> FindAsync(Paging paging,CancellationToken ct);
}