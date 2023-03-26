using Microsoft.Extensions.Logging;
using QuickToken.Core.Shared.Services;
using QuickToken.Database.Models;
using QuickToken.Database.Repositories;
using QuickToken.Facade.Domain.Models;

namespace QuickToken.Facade.Domain.Services;

public class AssetService : IAssetService
{
    private ILogger<AssetService> _logger;
    private IBlockchainService _blockchain;
    private IAssetSerialRepository _assetSerials;

    public AssetService(ILogger<AssetService> logger, IBlockchainService blockchain,
        IAssetSerialRepository assetSerials)
    {
        _blockchain = blockchain;
        _assetSerials = assetSerials;
        _logger = logger;
    }

    public async Task<(Guid supplyTransactionId, Guid securitizeTransactionId)>
        SecuritizeAsync(AssetSerialProposal proposal, CancellationToken ct)
    {
        var tokenPrice = proposal.Supply / proposal.Volume;
        var tokenIds = GenerateTokenIds(proposal.Volume);

        // Create serial in DB.
        var assetSerial =
            await _assetSerials.AddAsync(tokenIds, tokenPrice, proposal.DailyInterestRate, proposal.Duration, ct);

        // Initiate supply transaction.
        var supplyTransactionId = await _blockchain.MintCurrencyAsync(proposal.Supply, ct);

        // Initiate securitize transaction.
        var securitizeTransactionId =
            await _blockchain.MintAssetsSerialAsync(assetSerial.Id, tokenIds, assetSerial.Price,
                assetSerial.DailyInterestRate, assetSerial.IpoTimestamp, assetSerial.BurnTimestamp, ct);

        return (supplyTransactionId, securitizeTransactionId);
    }

    public async Task<AssetSerialAggregated> FindByIdAsync(Guid id, CancellationToken ct)
    {
        var assetSerial = await _assetSerials.FindByIdAsync(id, ct);
        if (assetSerial == null)
            return null;
        return CreateAssetSerialAggregated(assetSerial);
    }

    public async Task<AssetSerialAggregated[]> FindByAddressAsync(string address, int count, int shift,
        CancellationToken ct)
    {
        var assetSerials = await _assetSerials.FindByAddressAsync(address, new Paging(count, shift), ct);
        return assetSerials.Select(CreateAssetSerialAggregated).ToArray();
    }

    public async Task<AssetSerialAggregated[]> FindAsync(int count, int shift, CancellationToken ct)
    {
        var assetSerials = await _assetSerials.FindAsync(new Paging(count, shift), ct);
        return assetSerials.Select(CreateAssetSerialAggregated).ToArray();
    }

    private static AssetSerialAggregated CreateAssetSerialAggregated(AssetSerial assetSerial)
    {
        var aggregated = new AssetSerialAggregated
        {
            Id = assetSerial.Id,
            Price = assetSerial.Price,
            DailyInterestRate = assetSerial.DailyInterestRate,
            BurnTimestamp = assetSerial.BurnTimestamp
        };

        if (assetSerial.Assets is not null)
        {
            aggregated.Owners = assetSerial.Assets
                .Where(asset => asset.WalletId is not null)
                .GroupBy(asset => asset.WalletId)
                .ToDictionary(p => p.First().Wallet!.Address, p => p.Count());
        }

        return aggregated;
    }

    private static Guid[] GenerateTokenIds(int amount)
    {
        return Enumerable.Range(0, amount).Select(_ => Guid.NewGuid()).ToArray();
    }
}