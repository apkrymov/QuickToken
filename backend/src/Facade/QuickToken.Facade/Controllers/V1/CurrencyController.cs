using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickToken.Core.Shared.Services;
using QuickToken.Facade.Contracts;
using QuickToken.Shared.Web.RBAC;

namespace QuickToken.Facade.Controllers.V1;

/// <summary>
/// Currency operations
/// </summary>
[Route(BaseRoute + "currency")]
public class CurrencyController : ControllerV1Base
{
    private readonly ILogger<CurrencyController> _logger;
    private readonly IBlockchainService _blockchain;

    public CurrencyController(ILogger<CurrencyController> logger,
        IBlockchainService blockchain)
    {
        _logger = logger;
        _blockchain = blockchain;
    }

    /// <summary>
    /// Mint coins
    /// </summary>
    [Authorize(Policy = Policies.Operator)]
    [ProducesResponseType(typeof(TransactionStatusResponse), StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<IActionResult> CreateCurrencyAsync(CreateCurrencyRequest request, CancellationToken ct)
    {
        var transactionId = await _blockchain.MintCurrencyAsync(request.Amount, request.Address, ct);

        return CreatedAtAction("GetStatus", "Transaction", new
        {
            id = transactionId
        }, new TransactionStatusResponse
        {
            Id = transactionId
        });
    }
}