using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuickToken.Database.Configurations;
using QuickToken.Database.Models;

namespace QuickToken.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    
    public DbSet<Wallet> Wallets { get; set; }
    
    public DbSet<WalletSnapshot> WalletSnapshots { get; set; }
    
    public DbSet<Asset> Assets { get; set; }
    
    public DbSet<AssetSerial> AssetSerials { get; set; }
    
    public DbSet<BlockchainTransaction> BlockchainTransactions { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AccountConfiguration());
        builder.ApplyConfiguration(new BlockchainTransactionConfiguration());
        builder.ApplyConfiguration(new WalletConfiguration());
        builder.ApplyConfiguration(new WalletSnapshotConfiguration());
        builder.ApplyConfiguration(new AssetConfiguration());
        builder.ApplyConfiguration(new AssetSerialConfiguration());
        
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            ApplyDateTimeSqliteWorkaround(builder);
        }
    }

    private static void ApplyDateTimeSqliteWorkaround(ModelBuilder builder)
    {
        // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
        // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
        // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
        // use the DateTimeOffsetToBinaryConverter
        // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
        // This only supports millisecond precision, but should be sufficient for most use cases.
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                           || p.PropertyType == typeof(DateTimeOffset?));
            foreach (var property in properties)
            {
                builder
                    .Entity(entityType.Name)
                    .Property(property.Name)
                    .HasConversion(new DateTimeOffsetToBinaryConverter());
            }
        }
    }
}