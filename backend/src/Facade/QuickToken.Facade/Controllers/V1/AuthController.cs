using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickToken.Facade.Contracts;
using QuickToken.Facade.Domain.Services;

namespace QuickToken.Facade.Controllers.V1;

/// <summary>
/// Authorization gateway
/// </summary>
[AllowAnonymous]
[Route(BaseRoute + "auth")]
public class AuthController : ControllerV1Base
{
    private readonly ILogger<AuthController> _logger;
    private readonly IJwtTokenService _jwtToken;
    private readonly IAccountService _accounts;

    public AuthController(ILogger<AuthController> logger, IJwtTokenService jwtToken, 
        IAccountService accounts)
    {
        _logger = logger;
        _jwtToken = jwtToken;
        _accounts = accounts;
    }
    
    /// <summary>
    /// Authorization by wallet address
    /// </summary>
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [HttpPost("wallet")]
    public async Task<IActionResult> WalletAuthAsync(WalletAuthRequest walletAuth, CancellationToken ct)
    {
        var claims = await _accounts.WalletAuthAsync(walletAuth.Wallet, ct);
        var token = await _jwtToken.CreateAsync(claims, ct);
        return Ok(new AuthResponse
        {
            AccessToken = token
        });
    }
    
    /// <summary>
    /// Authorization by username and password
    /// </summary>
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [HttpPost("userpass")]
    public async Task<IActionResult> UserpassAuthAsync(UserpassAuthRequest userpassAuth, CancellationToken ct)
    {
        var claims = await _accounts.UserpassAuthAsync(userpassAuth.Login, userpassAuth.Password, ct);
        var token = await _jwtToken.CreateAsync(claims, ct);
        return Ok(new AuthResponse
        {
            AccessToken = token
        });
    }
}