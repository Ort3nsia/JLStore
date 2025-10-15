using JLStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JLStore.Infrastructure.Configuration;

public static class PersistenceRegistration
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config, IHostEnvironment env)
    {
        var provider = Environment.GetEnvironmentVariable("DB_PROVIDER")
                    ?? config["Database:Provider"]
                    ?? "sqlserver";

        if (provider.Equals("sqlserver", StringComparison.OrdinalIgnoreCase))
        {
            var cs = ConnectionStringFactory.BuildForSqlServer(config, env);
            services.AddDbContext<DataContext>(opt =>
                opt.UseSqlServer(cs, sql => {
                    sql.MigrationsAssembly("JLStore.Migrations.SqlServer");
                    sql.MigrationsHistoryTable("__EFMigrationsHistory_Sql", "dbo");
                })
            );
        }
        else if (provider.Equals("postgres", StringComparison.OrdinalIgnoreCase))
        {
            var cs = ConnectionStringFactory.BuildForPostgres(config, env);
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseNpgsql(cs, npg => {
                    npg.MigrationsAssembly("JLStore.Migrations.Postgres");
                    npg.MigrationsHistoryTable("__EFMigrationsHistory_Pg", "public");
                });
            });
        }
        else
        {
            throw new NotSupportedException($"Provider non supportato: {provider}");
        }

        return services;
    }
}
