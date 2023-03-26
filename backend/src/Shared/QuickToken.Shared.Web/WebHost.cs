using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using QuickToken.Database;
using QuickToken.Shared.Web.Options;
using QuickToken.Shared.Web.RBAC;
using Swashbuckle.AspNetCore.Filters;

namespace QuickToken.Shared.Web;

public class WebHost
{
    private readonly WebApplicationBuilder _internalBuilder;
    private WebApplication _internalApp;

    private readonly List<Type> _filters = new();

    private WebHost(string[] args)
    {
        _internalBuilder = WebApplication.CreateBuilder(args);
    }

    public static WebHost WithArgs(string[] args)
    {
        return new WebHost(args);
    }
    
    public WebHost WithFilter<TFilter>()
    {
        _filters.Add(typeof(TFilter));
        return this;
    }

    public async Task RunAsync()
    {
        Build();
        Configure();

        await _internalApp.RunAsync();
    }

    public WebHost ConfigureContainer(Action<HostBuilderContext, ContainerBuilder> func)
    {
        _internalBuilder.Host.ConfigureContainer(func);
        return this;
    }

    private void Configure()
    {
        ArgumentNullException.ThrowIfNull(_internalApp);

        _internalApp.Services.MigrateDatabase();

        _internalApp.UseSwagger();
        _internalApp.UseSwaggerUI();

        _internalApp.UseAuthentication();
        _internalApp.UseAuthorization();

        _internalApp.MapControllers();
    }

    private void Build()
    {
        ArgumentNullException.ThrowIfNull(_internalBuilder);

        _internalBuilder.Host.AddLogging();
        _internalBuilder.Host.AddServiceProvider();

        _internalBuilder.Services.AddControllers(p =>
        {
            foreach (var filter in _filters.Distinct())
            {
                p.Filters.Add(filter);
            }
        });
        _internalBuilder.Services.AddEndpointsApiExplorer();
        _internalBuilder.Services.AddSwaggerGen(options =>
        {
            options.MapType<TimeSpan>(() => new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("00:00:00")
            });
            
            var xmlFilename = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), includeControllerXmlComments: true);
            
            options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            options.OperationFilter<SecurityRequirementsOperationFilter>(true, "JWT");
            options.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "JWT"}
                    },
                    new string[] { }
                }
            });
        });

        var jwtTokenOptions = _internalBuilder.Configuration.GetSection("JwtToken").Get<JwtTokenOptions>();
        _internalBuilder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtTokenOptions.Issuer,
                    ValidateIssuer = true,
                    ValidAudience = jwtTokenOptions.Audience,
                    ValidateAudience = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = jwtTokenOptions.GetPublicKey()
                };
            });
        _internalBuilder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.Investor, policy =>
                policy.RequireRole(Roles.Investor));
            options.AddPolicy(Policies.Operator, policy =>
                policy.RequireRole(Roles.Operator));
            options.AddPolicy(Policies.Bank, policy =>
                policy.RequireRole(Roles.Bank));
            options.AddPolicy(Policies.Any, policy =>
                policy.RequireRole(Roles.Investor, Roles.Operator, Roles.Bank));
        });
        _internalBuilder.Services.AddDatabase(_internalBuilder.Configuration,
            _internalBuilder.Environment.IsDevelopment());

        _internalApp = _internalBuilder.Build();
    }
}