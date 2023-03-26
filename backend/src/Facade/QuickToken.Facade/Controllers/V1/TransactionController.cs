using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickToken.Core.Shared.Services;
using QuickToken.Facade.Contracts;
using QuickToken.Facade.Domain.Services;
using QuickToken.Shared.Web.RBAC;

namespace QuickToken.Facade.Controllers.V1;

/// <summary>
/// Blockchain transaction operations
/// </summary>
[Route(BaseRoute + "transaction")]
[Authorize(Policy = Policies.Any)]
public class TransactionController : ControllerV1Base
{
    private readonly ILogger<TransactionController> _logger;
    private readonly IBlockchainService _blockchain;
    private readonly IAccountService _accounts;

    public TransactionController(ILogger<TransactionController> logger,
        IBlockchainService blockchain, IAccountService accounts)
    {
        _logger = logger;
        _blockchain = blockchain;
        _accounts = accounts;
    }
    
    /// <summary>
    /// Checks status of executing blockchain transaction
    /// </summary>
    [ProducesResponseType(typeof(TransactionStatusResponse), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStatusAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var (isCompleted, payload) = await _blockchain.GetTransactionAsync(id, ct);
        var result = new TransactionStatusResponse
        {
            Id = id,
            State = isCompleted ? TransactionState.Completed : TransactionState.InProgress,
            Payload = payload
        };
        return Ok(result);
    }
}