using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickToken.Facade.Contracts;
using QuickToken.Facade.Domain.Models;
using QuickToken.Facade.Domain.Services;
using QuickToken.Shared.Web.RBAC;

namespace QuickToken.Facade.Controllers.V1;

/// <summary>
/// Assets operations
/// </summary>
[Route(BaseRoute + "asset")]
public class AssetController : ControllerV1Base
{
    private readonly ILogger<AssetController> _logger;
    private readonly IAssetService _assets;

    public AssetController(ILogger<AssetController> logger, IAssetService assets)
    {
        _logger = logger;
        _assets = assets;
    }

    /// <summary>
    /// Mint serial of assets
    /// </summary>
    [Authorize(Policy = Policies.Bank)]
    [ProducesResponseType(typeof(TransactionStatusResponse[]), StatusCodes.Status201Created)]
    [HttpPost("serial")]
    public async Task<IActionResult> CreateAssetSerialAsync(CreateAssetSerialRequest request, CancellationToken ct)
    {
        var proposal = new AssetSerialProposal
        {
            Supply = request.Supply,
            Volume = request.Volume,
            DailyInterestRate = request.DailyInterestRate,
            Duration = request.Duration
        };
        var (supplyTransactionId, securitizeTransactionId) = await _assets.SecuritizeAsync(proposal, ct);

        return CreatedAtAction("GetStatus", "Transaction", new
        {
            id = securitizeTransactionId
        }, new[]
        {
            new TransactionStatusResponse
            {
                Id = supplyTransactionId
            },
            new TransactionStatusResponse
            {
                Id = securitizeTransactionId
            }
        });
    }
    
    /// <summary>
    /// Get information about specified asset serial with known owners
    /// </summary>
    [Authorize(Policy = Policies.Operator)]
    [ProducesResponseType(typeof(AssetSerialOwnersResponse[]), StatusCodes.Status200OK)]
    [HttpGet("serial/{id}")]
    public async Task<IActionResult> GetAssetSerialOwnersAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var assetSerial = await _assets.FindByIdAsync(id, ct);
        if (assetSerial == null)
        {
            return new NotFoundResult();
        }
        return Ok(new AssetSerialOwnersResponse
        {
            Id = assetSerial.Id,
            Price = assetSerial.Price,
            DailyInterestRate = assetSerial.DailyInterestRate,
            BurnTimestamp = assetSerial.BurnTimestamp,
            Owners = assetSerial.Owners
        });
    }
    
    /// <summary>
    /// Get information about all minted asset serials
    /// </summary>
    [Authorize(Policy = Policies.Any)]
    [ProducesResponseType(typeof(AssetSerialResponse[]), StatusCodes.Status200OK)]
    [HttpGet("serial")]
    public async Task<IActionResult> GetAssetSerialsAsync([FromQuery]PaginatedRequest page, CancellationToken ct)
    {
        var aggregatedResult = await _assets.FindAsync(page.Count, page.Shift, ct);
        
        return Ok(aggregatedResult.Select(assetSerial => new AssetSerialResponse
        {
            Id = assetSerial.Id,
            Price = assetSerial.Price,
            DailyInterestRate = assetSerial.DailyInterestRate,
            BurnTimestamp = assetSerial.BurnTimestamp,
        }));
    }
}