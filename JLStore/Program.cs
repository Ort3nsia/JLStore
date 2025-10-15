using DotNetEnv;
using JLStore.Infrastructure.Configuration;
using JLStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ===== Carica .env solo in Dev e FUORI dal container =====
var runningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
if (builder.Environment.IsDevelopment() && !runningInContainer)
{
    var repoRoot = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, ".."));
    var envPath  = Path.Combine(repoRoot, ".env");
    if (File.Exists(envPath)) Env.Load(envPath);
}

// ===== Persistence (DbContext + provider) =====
builder.Services.AddPersistence(builder.Configuration, builder.Environment);

// ===== Servizi applicativi (Repo, Services, AutoMapper) =====
builder.Services.AddApplicationServices();

// ===== Web layer (Controllers, Swagger) =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JLStore API",
        Version = "v1",
        Description = "Minimal JLStore Web API"
    });
});

var app = builder.Build();

// ===== MIGRATE + SEED allo startup (solo Dev) =====
if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    var sp     = scope.ServiceProvider;
    var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
    var db     = sp.GetRequiredService<DataContext>();

    var autoMigrate = Environment.GetEnvironmentVariable("AUTO_MIGRATE") == "1";

    if (autoMigrate)
    {
        try
        {
            var pending = await db.Database.GetPendingMigrationsAsync();
            logger.LogInformation("Pending migrations: {Count}", pending.Count());
            if (pending.Any())
            {
                logger.LogInformation("Applying migrations...");
                await db.Database.MigrateAsync();
                logger.LogInformation("Migrations applied.");
            }
        }
        catch (System.IO.FileNotFoundException ex)
        {
            // Migrations assembly non presente nell'immagine: logga e prosegui
            logger.LogWarning(ex, "AUTO_MIGRATE abilitato ma migrations assembly non presente. Salto migrazione runtime.");
        }
    }

    logger.LogInformation("Running seed...");
    await DataSeed.EnsureSeedAsync(db);
    logger.LogInformation("Seed done.");
}

// ===== Pipeline =====
// In Dev dentro container evitiamo HTTPS redirect (stai esponendo solo HTTP:5250)
var isDev = app.Environment.IsDevelopment();
var inContainerNow = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
if (!(isDev && inContainerNow))
{
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.RoutePrefix = string.Empty; // Swagger UI sulla root "/"
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "JLStore API v1");
});

app.MapGet("/health", () => Results.Ok(new { ok = true, ts = DateTimeOffset.UtcNow }));

app.MapControllers();

await app.RunAsync();
