using JLStore.Infrastructure.Configuration;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Carica .env solo in Dev e solo FUORI dai container (come facevi già)
var runningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
if (builder.Environment.IsDevelopment() && !runningInContainer)
{
    var repoRoot = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, ".."));
    var envPath = Path.Combine(repoRoot, ".env");
    if (File.Exists(envPath))
        Env.Load(envPath); // richiede package DotNetEnv
}

// Registrazioni
builder.Services
    .AddPersistence(builder.Configuration, builder.Environment)
    .AddApplicationServices();

var app = builder.Build();

// Pipeline + DB migrate/seed
app.UseAppDefaults();
await app.ApplyMigrationsAndSeedAsync();

await app.RunAsync();
