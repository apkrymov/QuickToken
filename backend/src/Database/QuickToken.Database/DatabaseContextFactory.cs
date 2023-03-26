using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace QuickToken.Database;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

        if (args.Contains("--sqlite"))
        {
            optionsBuilder.UseSqlite();
        }
        
        if (args.Contains("--postgres"))
        {
            optionsBuilder.UseNpgsql();
        }

        return new DatabaseContext(optionsBuilder.Options);
    }
}