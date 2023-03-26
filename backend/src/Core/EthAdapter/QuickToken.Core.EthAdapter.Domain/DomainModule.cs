using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.Extensions.Configuration;
using QuickToken.Core.EthAdapter.Domain.Options;
using QuickToken.Core.EthAdapter.Domain.Rpc;

namespace QuickToken.Core.EthAdapter.Domain;

public class DomainModule : Module
{
    private readonly IConfiguration _configuration;

    public DomainModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        var web3GateOptions = _configuration.GetSection("Web3Gate").Get<Web3GateOptions>();
        builder.RegisterInstance(web3GateOptions);

        var accountsOptions = _configuration.GetSection("AccountsByRole").Get<AccountsByRoleOptions>();
        builder.RegisterInstance(accountsOptions);

        builder.RegisterType<Web3Factory>().As<IWeb3Factory>();
        RegisterMediator(builder);
    }

    private static void RegisterMediator(ContainerBuilder builder)
    {
        var configuration = MediatRConfigurationBuilder
            .Create(typeof(DomainModule).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();
        builder.RegisterMediatR(configuration);
    }
}