using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickToken.Database.Options;

namespace QuickToken.Database;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, bool isLocalRun = false)
    {
        var dbOptions = configuration.GetSection("Database").Get<DatabaseConnectionOptions>();
        return services.AddDbContext<DatabaseContext>(p =>
        {
            if (isLocalRun)
            {
                var dbFolder = Path.Join(Environment.ExpandEnvironmentVariables("%ProgramData%"), "QuickToken");
                Directory.CreateDirectory(dbFolder);
                
                p.UseSqlite($"Data Source={Path.Join(dbFolder, "local.db")}"); 
            }
            else
            {
                p.UseNpgsql(dbOptions.GetConnectionString());
            }
        });
    }

    public static void MigrateDatabase(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        dataContext.Database.Migrate();
    }
}