using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace QuickToken.Shared.Worker;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder RegisterWorker<TWorker, TWorkerOptions>(this ContainerBuilder builder, IConfiguration configuration,
        string optionsSection)
        where TWorker : notnull
    {
        var options = configuration.GetSection(optionsSection).Get<TWorkerOptions>();
        builder.RegisterType<TWorker>()
            .WithParameter(
                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(TWorkerOptions),
                    (pi, ctx) => options))
            .As<IHostedService>()
            .SingleInstance();
        return builder;
    }
}