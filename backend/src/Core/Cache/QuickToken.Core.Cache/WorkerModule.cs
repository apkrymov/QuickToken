using Autofac;
using QuickToken.Core.Cache.Options;
using QuickToken.Core.Cache.Workers;
using QuickToken.Shared.Worker;

namespace QuickToken.Core.Cache;

public class WorkerModule : Module
{
    private readonly IConfiguration _configuration;

    public WorkerModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterWorker<WalletWorker, CacheWorkerOptions>(_configuration, "WalletWorker");
    }
}