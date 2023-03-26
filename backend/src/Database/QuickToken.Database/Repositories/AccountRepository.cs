using Autofac.Features.OwnedInstances;
using Microsoft.EntityFrameworkCore;
using QuickToken.Database.Hashing;
using QuickToken.Database.Models;
using QuickToken.Shared.Exceptions;

namespace QuickToken.Database.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly Func<Owned<DatabaseContext>> _dbFactory;

    public AccountRepository(Func<Owned<DatabaseContext>> dbFactory)
    {
        _dbFactory = dbFactory;
    }
    
    public async Task<Account> AddByWalletAsync(string wallet, string roles, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var account = new Account
        {
            Roles = roles,
            Wallet = new Wallet
            {
                Address = wallet
            },
            CreatedAt = DateTimeOffset.Now
        };

        var result = await db.Accounts.AddAsync(account, ct);
        await db.SaveChangesAsync(ct);

        return result.Entity;
    }
    
    public async Task<Account> AddByUserpassAsync(string login, string password, string roles, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var account = new Account
        {
            Roles = roles,
            Login = login,
            Password = PBKDF2.Hash(password),
            CreatedAt = DateTimeOffset.Now
        };

        var result = await db.Accounts.AddAsync(account, ct);
        await db.SaveChangesAsync(ct);

        return result.Entity;
    }

    public async Task<Account> FindByUserpassAsync(string login, string password, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var account = await db.Accounts
            .Where(p => p.Login == login)
            .AsNoTracking()
            .SingleOrDefaultAsync(ct);

        if (account?.Password is null)
        {
            throw new EntityNotFoundException();
        }

        if (!PBKDF2.Verify(password, account.Password))
        {
            throw new UnauthorizedAccessException();
        }

        return account;
    }

    public async Task<Account> FindByWalletAsync(string wallet, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var account = await db.Accounts
            .Include(p => p.Wallet)
            .Where(p => p.Wallet.Address == wallet)
            .AsNoTracking()
            .SingleOrDefaultAsync(ct);

        if (account is null)
        {
            throw new EntityNotFoundException();
        }

        return account;
    }

    public async Task<Account> FindAsync(Guid id, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        return await db.Accounts
            .Include(p => p.Wallet)
            .ThenInclude(p => p.Assets)
            .AsNoTracking()
            .SingleAsync(p => p.Id == id, ct);
    }
    
    public async Task<Account> UpdateLathAuthAsync(Guid id, CancellationToken ct)
    {
        var db = _dbFactory().Value;
        var account = await db.Accounts.SingleAsync(p => p.Id == id, ct);
        account.LastAuthAt = DateTimeOffset.Now;
        await db.SaveChangesAsync(ct);
        return account;
    }
}