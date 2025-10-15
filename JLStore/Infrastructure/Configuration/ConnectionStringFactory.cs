using Microsoft.Data.SqlClient;
using Npgsql;

namespace JLStore.Infrastructure.Configuration;

public static class ConnectionStringFactory
{
    public static string BuildForSqlServer(IConfiguration config, IHostEnvironment env)
    {
        var runningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

        // ENV > appsettings:Default
        var csFromEnv = Environment.GetEnvironmentVariable("ConnectionStrings__Default")
                    ?? Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION");

        var baseCs = csFromEnv
                  ?? config.GetConnectionString("Default")
                  ?? throw new InvalidOperationException("ConnectionStrings:Default mancante.");

        var csb = new SqlConnectionStringBuilder(baseCs);
        
        // Solo se il DataSource punta a localhost/127.0.0.1, sostituiscilo
        if (runningInContainer)
        {
            var ds = csb.DataSource?.Trim();
            if (string.Equals(ds, "localhost,1433", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ds, "127.0.0.1,1433", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ds, "host.docker.internal,1433", StringComparison.OrdinalIgnoreCase))
            {
                csb.DataSource = "mssql,1433";
            }
        }

        // Imposta sempre Encrypt + TrustServerCertificate se non presenti
        csb.Encrypt = true;
        csb.TrustServerCertificate = true;

        var containerRuntime = Environment.GetEnvironmentVariable("CONTAINER_RUNTIME") ?? "docker";
        var hostGateway = containerRuntime.Equals("podman", StringComparison.OrdinalIgnoreCase)
            ? "host.containers.internal"
            : "host.docker.internal";

        bool isLocalDataSource =
            string.IsNullOrWhiteSpace(csb.DataSource) ||
            csb.DataSource.Equals(".", StringComparison.OrdinalIgnoreCase) ||
            csb.DataSource.Contains("(localdb)", StringComparison.OrdinalIgnoreCase) ||
            csb.DataSource.Contains("localhost", StringComparison.OrdinalIgnoreCase) ||
            csb.DataSource.StartsWith("127.0.0.1");

        if (runningInContainer && isLocalDataSource)
        {
            var host = Environment.GetEnvironmentVariable("SQLSERVER_HOST") ?? hostGateway;
            var port = Environment.GetEnvironmentVariable("SQLSERVER_PORT") ?? "1433";
            csb.DataSource = $"{host},{port}";
        }

        if (string.IsNullOrWhiteSpace(csb.Password))
        {
            var pwd = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD")
                   ?? Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD");
            if (!string.IsNullOrWhiteSpace(pwd))
                csb.Password = pwd;
        }

        csb.TrustServerCertificate = true;
        if (!csb.ContainsKey("Encrypt"))
            csb.Encrypt = false;

        return csb.ConnectionString;
    }

    public static string BuildForPostgres(IConfiguration config, IHostEnvironment env)
    {
        var runningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

        var csFromEnv = Environment.GetEnvironmentVariable("ConnectionStrings__Default")
                ?? Environment.GetEnvironmentVariable("POSTGRES_CONNECTION");

        var baseCs = csFromEnv
                ?? config.GetConnectionString("Default")
                ?? throw new InvalidOperationException("ConnectionStrings:Default mancante.");

        var csb = new NpgsqlConnectionStringBuilder(baseCs);

        // host/port override se sei in container e la cs punta al localhost
        var containerRuntime = Environment.GetEnvironmentVariable("CONTAINER_RUNTIME") ?? "docker";
        var hostGateway = containerRuntime.Equals("podman", StringComparison.OrdinalIgnoreCase)
            ? "host.containers.internal"
            : "host.docker.internal";

        bool isLocalHost =
            string.IsNullOrWhiteSpace(csb.Host) ||
            csb.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) ||
            csb.Host.StartsWith("127.0.0.1");

        if (runningInContainer && isLocalHost)
        {
            csb.Host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? hostGateway;
            csb.Port = int.Parse(Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432");
        }

        if (string.IsNullOrWhiteSpace(csb.Password))
        {
            var pwd = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
            if (!string.IsNullOrWhiteSpace(pwd))
                csb.Password = pwd;
        }

        // Imposta SSL solo se NON era già nella CS di input
        var hasSslInInput =
            baseCs.IndexOf("ssl mode", StringComparison.OrdinalIgnoreCase) >= 0 ||
            baseCs.IndexOf("sslmode", StringComparison.OrdinalIgnoreCase) >= 0;

        if (!hasSslInInput)
            csb.SslMode = SslMode.Prefer;

        // Comodo in dev/compose
        csb.TrustServerCertificate = true;

        return csb.ConnectionString;
    }   
}
