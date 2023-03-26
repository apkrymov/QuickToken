using QuickToken.Facade.Domain.Models;

namespace QuickToken.Facade.Domain.Services;

public interface IAssetService
{
    public Task<(Guid supplyTransactionId, Guid securitizeTransactionId)> SecuritizeAsync(AssetSerialProposal assetSerial,
        CancellationToken ct);

    public Task<AssetSerialAggregated> FindByIdAsync(Guid id, CancellationToken ct);
    
    public Task<AssetSerialAggregated[]> FindByAddressAsync(string address, int count, int shift, CancellationToken ct);
    
    public Task<AssetSerialAggregated[]> FindAsync(int count, int shift, CancellationToken ct);
}