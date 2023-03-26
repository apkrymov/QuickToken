using Autofac;
using QuickToken.Database.Repositories;

namespace QuickToken.Database;

public class DatabaseModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AccountRepository>()
            .As<IAccountRepository>()
            .SingleInstance();
        
        builder.RegisterType<WalletRepository>()
            .As<IWalletRepository>()
            .SingleInstance();
        
        builder.RegisterType<WalletSnapshotRepository>()
            .As<IWalletSnapshotRepository>()
            .SingleInstance();
        
        builder.RegisterType<AssetRepository>()
            .As<IAssetRepository>()
            .SingleInstance();
        
        builder.RegisterType<AssetSerialRepository>()
            .As<IAssetSerialRepository>()
            .SingleInstance();

        builder.RegisterType<BlockchainTransactionRepository>()
            .As<IBlockchainTransactionRepository>()
            .SingleInstance();
    }
}