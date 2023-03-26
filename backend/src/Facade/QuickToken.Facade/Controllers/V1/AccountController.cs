using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickToken.Core.Shared.Services;
using QuickToken.Database.Repositories;
using QuickToken.Facade.Contracts;
using QuickToken.Facade.Domain.Services;
using QuickToken.Shared.Web.RBAC;

namespace QuickToken.Facade.Controllers.V1;

/// <summary>
/// Account operations
/// </summary>
[Route(BaseRoute + "account")]
public class AccountController : ControllerV1Base
{
    private readonly ILogger<AccountController> _logger;
    private readonly IBlockchainService _blockchain;
    private readonly IWalletSnapshotRepository _walletSnapshots;
    private readonly IAccountService _accounts;

    public AccountController(ILogger<AccountController> logger,
        IBlockchainService blockchain, IAccountService accounts, IWalletSnapshotRepository walletSnapshots)
    {
        _logger = logger;
        _blockchain = blockchain;
        _accounts = accounts;
        _walletSnapshots = walletSnapshots;
    }

    /// <summary>
    /// Details of account
    /// </summary>]
    [Authorize(Policy = Policies.Any)]
    [ProducesResponseType(typeof(AccountDetailsResponse), StatusCodes.Status200OK)]
    [HttpGet("details")]
    public async Task<IActionResult> DetailsAsync(CancellationToken ct)
    {
        var result = new AccountDetailsResponse
        {
            Id = User.Id(),
            Roles = User.Roles()
        };
        return Ok(result);
    }

    /// <summary>
    /// Balances of account
    /// </summary>
    [Authorize(Policy = Policies.Investor)]
    [ProducesResponseType(typeof(AccountBalanceResponse), StatusCodes.Status200OK)]
    [HttpGet("balance")]
    public async Task<IActionResult> BalanceAsync(CancellationToken ct)
    {
        var wallet = await _accounts.GetWallet(User.Id(), ct);
        var result = new AccountBalanceResponse
        {
            Eth = wallet.Eth,
            Currency = wallet.Currency,
            Assets = wallet.Assets.Select(p => new AssetResponse
            {
                TokenId = p.TokenId,
                SerialId = p.AssetSerialId
            }).ToArray()
        };
        return Ok(result);
    }

    /// <summary>
    /// History of balance changes
    /// </summary>
    [Authorize(Policy = Policies.Investor)]
    [ProducesResponseType(typeof(BalanceSnapshotResponse), StatusCodes.Status200OK)]
    [HttpGet("balance/history")]
    public async Task<IActionResult> BalanceAsync([FromQuery] PaginatedRequest request, CancellationToken ct)
    {
        var wallet = await _accounts.GetWallet(User.Id(), ct);
        var walletSnapshots = await _walletSnapshots.GetAsync(wallet.Id, new Paging(request.Count, request.Shift), ct);
        return Ok(walletSnapshots.Select(p => new BalanceSnapshotResponse
        {
            Currency = p.Currency,
            Eth = p.Eth,
            Timestamp = p.Timestamp
        }));
    }
}