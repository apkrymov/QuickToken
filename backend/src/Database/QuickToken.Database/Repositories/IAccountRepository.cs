using QuickToken.Database.Models;

namespace QuickToken.Database.Repositories;

public interface IAccountRepository
{
    public Task<Account> AddByWalletAsync(string wallet, string roles, CancellationToken ct);

    public Task<Account> AddByUserpassAsync(string login, string password, string roles, CancellationToken ct);
    
    public Task<Account> FindByUserpassAsync(string login, string password, CancellationToken ct);
    
    public Task<Account> FindByWalletAsync(string wallet, CancellationToken ct);
    
    public Task<Account> FindAsync(Guid id, CancellationToken ct);

    public Task<Account> UpdateLathAuthAsync(Guid id, CancellationToken ct);
}