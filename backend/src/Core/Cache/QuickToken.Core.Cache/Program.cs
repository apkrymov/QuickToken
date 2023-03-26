using Autofac;
using QuickToken.Core.Cache;
using QuickToken.Core.Shared.Services;
using QuickToken.Database;
using QuickToken.Shared.Worker;

await WorkerHost
    .WithArgs(args)
    .ConfigureContainer((context, builder) =>
    {
        builder.RegisterModule(new DatabaseModule());
        builder.RegisterModule(new SharedModule(context.Configuration));
        builder.RegisterModule(new WorkerModule(context.Configuration));
    })
    .RunAsync();