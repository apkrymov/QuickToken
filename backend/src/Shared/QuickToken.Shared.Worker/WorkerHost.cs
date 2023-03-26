using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuickToken.Database;

namespace QuickToken.Shared.Worker;

public class WorkerHost
{
    private readonly IHostBuilder _internalBuilder;
    private IHost _internalApp;

    private WorkerHost(string[] args)
    {
        _internalBuilder = Host.CreateDefaultBuilder(args);
    }

    public static WorkerHost WithArgs(string[] args)
    {
        return new WorkerHost(args);
    }

    public async Task RunAsync()
    {
        Build();
        _internalApp.Services.MigrateDatabase();
        
        await _internalApp.RunAsync();
    }

    public WorkerHost ConfigureServices(Action<HostBuilderContext, IServiceCollection> func)
    {
        _internalBuilder.ConfigureServices(func);
        return this;
    }

    public WorkerHost ConfigureContainer(Action<HostBuilderContext, ContainerBuilder> func)
    {
        _internalBuilder.ConfigureContainer(func);
        return this;
    }

    private void Build()
    {
        ArgumentNullException.ThrowIfNull(_internalBuilder);

        _internalBuilder.AddLogging();
        _internalBuilder.AddServiceProvider();
        _internalBuilder.ConfigureServices((context, services) =>
        {
            services.AddDatabase(context.Configuration, context.HostingEnvironment.IsDevelopment());
        });

        _internalApp = _internalBuilder.Build();
    }
}