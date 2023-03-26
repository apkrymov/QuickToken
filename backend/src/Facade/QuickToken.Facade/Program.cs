using Autofac;
using QuickToken.Database;
using QuickToken.Facade.Domain;
using QuickToken.Facade.Filters;
using QuickToken.Shared.Web;

await WebHost
    .WithArgs(args)
    .WithFilter<ExceptionFilter>()
    .ConfigureContainer((context, builder) =>
    {
        builder.RegisterModule(new DatabaseModule());
        builder.RegisterModule(new DomainModule(context.Configuration));
    })
    .RunAsync();