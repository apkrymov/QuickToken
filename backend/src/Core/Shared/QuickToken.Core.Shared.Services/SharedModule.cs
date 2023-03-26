using Autofac;
using Microsoft.Extensions.Configuration;
using QuickToken.Core.Shared.Services.Polling;

namespace QuickToken.Core.Shared.Services;

public class SharedModule : Module
{
    private readonly IConfiguration _configuration;

    public SharedModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        var transactionPollingOptions =
            _configuration.GetSection("TransactionPolling").Get<TransactionPollingOptions>();
        if (transactionPollingOptions is not null)
        {
            builder.RegisterInstance(transactionPollingOptions);
            builder.RegisterType<TransactionPollingService>()
                .As<ITransactionPollingService>();
        }

        builder.RegisterType<BlockchainService>()
            .As<IBlockchainService>()
            .SingleInstance();
    }
}