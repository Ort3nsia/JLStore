using Microsoft.Data.SqlClient;

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
}
