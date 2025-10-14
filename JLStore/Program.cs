using JLStore.Domain.Repositories;
using JLStore.Domain.Services;
using JLStore.Infrastructure.Data;
using JLStore.Infrastructure.Repositories;
using JLStore.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Rileva se stai girando dentro un container
var runningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

// In Development, FUORI dal container carica il .env della repo (per MSSQL_SA_PASSWORD)
if (builder.Environment.IsDevelopment() && !runningInContainer)
{
    var repoRoot = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, ".."));
    var envPath = Path.Combine(repoRoot, ".env");
    if (File.Exists(envPath))
    {
        // Richiede: dotnet add JLStore package DotNetEnv
        DotNetEnv.Env.Load(envPath);
    }
}

// Connection string base dai settings
// Connection string base dai settings
// Se in compose o nel dev-container passi ConnectionStrings__Default / SQLSERVER_CONNECTION via env, queste sovrascrivono
var csFromEnv = Environment.GetEnvironmentVariable("ConnectionStrings__Default")
              ?? Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION");

var baseCs = csFromEnv
            ?? builder.Configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("ConnectionStrings:Default mancante.");

var csb = new SqlConnectionStringBuilder(baseCs);

// Sei in un container? (variabile già sopra)
var containerRuntime = Environment.GetEnvironmentVariable("CONTAINER_RUNTIME") ?? "docker";
var hostGateway = containerRuntime.Equals("podman", StringComparison.OrdinalIgnoreCase)
    ? "host.containers.internal"
    : "host.docker.internal";

// Se la DataSource è "locale" e siamo in container, forza host/porta
bool isLocalDataSource =
    string.IsNullOrWhiteSpace(csb.DataSource) ||
    csb.DataSource.Equals(".", StringComparison.OrdinalIgnoreCase) ||
    csb.DataSource.Contains("(localdb)", StringComparison.OrdinalIgnoreCase) ||
    csb.DataSource.Contains("localhost", StringComparison.OrdinalIgnoreCase) ||
    csb.DataSource.StartsWith("127.0.0.1");

if (runningInContainer && isLocalDataSource)
{
    // Se unisci il dev-container alla rete del compose, passa SQLSERVER_HOST (es. "jlstore-mssql");
    // altrimenti userà l’host gateway (richiede ports esposte, es. 1433:1433)
    var host = Environment.GetEnvironmentVariable("SQLSERVER_HOST") ?? hostGateway;
    var port = Environment.GetEnvironmentVariable("SQLSERVER_PORT") ?? "1433";
    csb.DataSource = $"{host},{port}";
}

// Password: usa quella nella CS, altrimenti prova da ENV (utile in dev-container)
if (string.IsNullOrWhiteSpace(csb.Password))
{
    var pwd = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD")
           ?? Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD");
    if (!string.IsNullOrWhiteSpace(pwd))
        csb.Password = pwd;
}

// Dev: evita rogne di certificato; se Encrypt mancante, disattivalo (non tocco se già impostato)
csb.TrustServerCertificate = true;
if (!csb.ContainsKey("Encrypt"))
{
    csb.Encrypt = false;
}

var finalCs = csb.ConnectionString;


// DI
builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);

// EF Core
builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseSqlServer(finalCs, sql => sql.EnableRetryOnFailure())
);

// Repo/Servizi
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<CustomerProfile>();
});

// API + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger in Development (o forzato via env)
if (app.Environment.IsDevelopment() ||
    Environment.GetEnvironmentVariable("FORCE_SWAGGER") == "1")
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "JLStore API v1");
        o.RoutePrefix = string.Empty; // UI su "/"
    });
}

// Applica automaticamente le migration in Development, con un piccolo retry (utile in compose)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();

    var attempts = 0;
    while (true)
    {
        try
        {
            await db.Database.MigrateAsync();
            break;
        }
        catch (SqlException) when (attempts++ < 10)
        {
            // Il DB potrebbe non essere ancora pronto: aspetta e ritenta
            await Task.Delay(TimeSpan.FromSeconds(3));
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();

// DIAGNOSTICA
app.MapGet("/diag/dbinfo", async (DataContext db) =>
{
    await using var cmd = db.Database.GetDbConnection().CreateCommand();
    cmd.CommandText = "SELECT DB_NAME() as Db, @@SERVERNAME as ServerName, SUSER_SNAME() as LoginName, @@SPID as Spid";
    await db.Database.OpenConnectionAsync();
    await using var r = await cmd.ExecuteReaderAsync();
    var rows = new List<Dictionary<string, object>>();
    while (await r.ReadAsync())
        rows.Add(Enumerable.Range(0, r.FieldCount).ToDictionary(i => r.GetName(i), i => r.GetValue(i)));
    return Results.Ok(rows);
});

app.MapGet("/diag/customers-last", async (DataContext db) =>
{
    var list = await db.Customers
        .AsNoTracking()
        .OrderByDescending(c => c.ID)
        .Select(c => new { c.ID, c.Name, c.Surname })
        .Take(5)
        .ToListAsync();
    return Results.Ok(list);
});

// Seeding (dopo la migrate)
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<DataContext>();
    await DataSeed.EnsureSeedAsync(ctx);
}

await app.RunAsync();
