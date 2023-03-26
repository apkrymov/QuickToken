using Autofac;
using QuickToken.Core.EthAdapter;
using QuickToken.Core.EthAdapter.Domain;
using QuickToken.Database;
using QuickToken.Shared.Worker;

await WorkerHost
    .WithArgs(args)
    .ConfigureContainer((context, builder) =>
    {
        builder.RegisterModule(new DatabaseModule());
        builder.RegisterModule(new WorkerModule(context.Configuration));
        builder.RegisterModule(new DomainModule(context.Configuration));
    })
    .RunAsync();