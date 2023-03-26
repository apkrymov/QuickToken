using QuickToken.Database.Models;
using QuickToken.Database.Repositories;
using QuickToken.Shared.Exceptions;
using QuickToken.Shared.Web.Models;
using QuickToken.Shared.Web.RBAC;

namespace QuickToken.Facade.Domain.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accounts;
    private readonly IWalletRepository _wallets;

    public AccountService(IAccountRepository accounts, IWalletRepository wallets)
    {
        _accounts = accounts;
        _wallets = wallets;
    }

    public async Task<JwtClaims> WalletAuthAsync(string wallet, CancellationToken ct)
    {
        Account? account;
        try
        {
            account = await _accounts.FindByWalletAsync(wallet, ct);
        }
        catch (EntityNotFoundException)
        {
            account = await _accounts.AddByWalletAsync(wallet, Roles.Investor, ct);
        }

        await _accounts.UpdateLathAuthAsync(account.Id, ct);
        var claims = new JwtClaims
        {
            Id = account.Id,
            Roles = account.Roles.Split(",")
        };
        return claims;
    }

    public async Task<Wallet> GetWallet(Guid id, CancellationToken ct)
    {
        var account = await _accounts.FindAsync(id, ct);
        await _wallets.SetCacheUpdateAsync(account.Wallet.Id, ct);
        return account.Wallet;
    }

    public async Task<JwtClaims> UserpassAuthAsync(string login, string password, CancellationToken ct)
    {
        Account? account;
        try
        {
            account = await _accounts.FindByUserpassAsync(login, password, ct);
        }
        catch (EntityNotFoundException)
        {
            account = await _accounts.AddByUserpassAsync(login, password, string.Empty, ct);
        }
        
        await _accounts.UpdateLathAuthAsync(account.Id, ct);
        var claims = new JwtClaims
        {
            Id = account.Id,
            Roles = account.Roles.Split(",")
        };
        return claims;
    }
}