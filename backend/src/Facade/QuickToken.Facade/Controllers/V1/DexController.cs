using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickToken.Facade.Contracts;
using QuickToken.Facade.Domain.Services;
using QuickToken.Shared.Eth;
using QuickToken.Shared.Web.RBAC;

namespace QuickToken.Facade.Controllers.V1;

/// <summary>
/// Decentralized exchange operations
/// </summary>
[Route(BaseRoute + "dex")]
public class DexController : ControllerV1Base
{
    private readonly ILogger<DexController> _logger;
    private readonly IAssetService _assets;

    public DexController(ILogger<DexController> logger, IAssetService assets)
    {
        _logger = logger;
        _assets = assets;
    }

    /// <summary>
    /// Get information about in-stock assets
    /// </summary>
    [Authorize(Policy = Policies.Any)]
    [ProducesResponseType(typeof(DexAssetSerialResponse[]), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetInStockSerialsAsync([FromQuery] PaginatedRequest page, CancellationToken ct)
    {
        var aggregatedResult = await _assets.FindByAddressAsync(DexContract.Address, page.Count, page.Shift, ct);

        return Ok(aggregatedResult.Select(assetSerial => new DexAssetSerialResponse
        {
            Id = assetSerial.Id,
            Price = assetSerial.Price,
            DailyInterestRate = assetSerial.DailyInterestRate,
            BurnTimestamp = assetSerial.BurnTimestamp,
            InStock = assetSerial.Owners.GetValueOrDefault(DexContract.Address, 0)
        }));
    }
}