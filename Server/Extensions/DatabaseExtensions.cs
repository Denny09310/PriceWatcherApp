using Microsoft.EntityFrameworkCore;

namespace PriceWatcher.Server.Extensions;

static class DatabaseExtensions
{
    public static IServiceCollection RegisterDbContext<T>(this IServiceCollection services, string connectionName) where T : DbContext
    {
        return services.AddPooledDbContextFactory<T>((sp, opt) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString(connectionName)
                ?? throw new ArgumentException("Cannot find selected connection string", nameof(connectionName));

            opt.UseSqlite(Environment.ExpandEnvironmentVariables(connectionString));
        });
    }
}
