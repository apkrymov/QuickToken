using QuickToken.Shared.Web.Models;

namespace QuickToken.Facade.Domain.Services;

public interface IJwtTokenService
{
    public Task<string> CreateAsync(JwtClaims claims, CancellationToken ct);
}