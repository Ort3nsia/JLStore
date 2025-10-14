using JLStore.Infrastructure.Configuration;
using JLStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JLStore.Infrastructure.Configuration;

public static class PersistenceRegistration
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config, IHostEnvironment env)
    {
        // In futuro potrai leggere anche "Database:Provider" o ENV DB_PROVIDER
        var provider = Environment.GetEnvironmentVariable("DB_PROVIDER")
                    ?? config["Database:Provider"]
                    ?? "sqlserver";

        if (provider.Equals("sqlserver", StringComparison.OrdinalIgnoreCase))
        {
            var cs = ConnectionStringFactory.BuildForSqlServer(config, env);
            services.AddDbContext<DataContext>(opt =>
                opt.UseSqlServer(cs, sql => sql.EnableRetryOnFailure()));
        }
        else
        {
            throw new NotSupportedException($"Provider non supportato: {provider}");
        }

        return services;
    }
}
