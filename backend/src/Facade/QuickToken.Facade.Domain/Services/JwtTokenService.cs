using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using QuickToken.Shared.Web.Models;
using QuickToken.Shared.Web.Options;

namespace QuickToken.Facade.Domain.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly ILogger<JwtTokenService> _logger;
    private readonly JwtTokenOptions _options;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    
    public JwtTokenService(ILogger<JwtTokenService> logger, JwtTokenOptions options)
    {
        _logger = logger;
        _options = options;
        
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public async Task<string> CreateAsync(JwtClaims claims, CancellationToken ct)
    {
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: GetClaims(claims),
            expires: DateTime.Now + _options.MaxExpiration,
            signingCredentials: new SigningCredentials(_options.GetPrivateKey(), SecurityAlgorithms.RsaSha512)
        );
        return _tokenHandler.WriteToken(token);
    }

    private Claim[] GetClaims(JwtClaims claims)
    {
        var result = new List<Claim>();

        foreach (var role in claims.Roles)
        {
            result.Add(new Claim(ClaimTypes.Role, role));
        }
        
        result.Add(new Claim(ClaimTypes.NameIdentifier, claims.Id.ToString()));
        
        return result.ToArray();
    }
}