using Autofac;
using QuickToken.Core.EthAdapter.Workers;
using QuickToken.Shared.Worker;
using QuickToken.Shared.Worker.Options;

namespace QuickToken.Core.EthAdapter;

public class WorkerModule : Module
{
    private readonly IConfiguration _configuration;

    public WorkerModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterWorker<HandleWorker, BaseWorkerOptions>(_configuration, "HandleWorker");
        builder.RegisterWorker<TrackProgressWorker, BaseWorkerOptions>(_configuration, "TrackProgressWorker");
    }
}