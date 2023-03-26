using Autofac;
using Microsoft.Extensions.Configuration;
using QuickToken.Core.Shared.Services;
using QuickToken.Facade.Domain.Services;
using QuickToken.Shared.Web.Options;

namespace QuickToken.Facade.Domain;

public class DomainModule : Module
{
    private readonly IConfiguration _configuration;

    public DomainModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        var jwtTokenOptions = _configuration.GetSection("JwtToken").Get<JwtTokenOptions>();
        builder.RegisterInstance(jwtTokenOptions);
        
        builder.RegisterType<JwtTokenService>()
            .As<IJwtTokenService>()
            .SingleInstance();
        
        builder.RegisterType<AccountService>()
            .As<IAccountService>()
            .SingleInstance();

        builder.RegisterType<AssetService>()
            .As<IAssetService>()
            .SingleInstance();

        builder.RegisterModule(new SharedModule(_configuration));
    }
}