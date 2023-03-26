using QuickToken.Database.Models;
using QuickToken.Shared.Web.Models;

namespace QuickToken.Facade.Domain.Services;

public interface IAccountService
{
    public Task<JwtClaims> WalletAuthAsync(string wallet, CancellationToken ct);
    
    public Task<Wallet> GetWallet(Guid accountId, CancellationToken ct);
    
    public Task<JwtClaims> UserpassAuthAsync(string login, string password, CancellationToken ct);
}