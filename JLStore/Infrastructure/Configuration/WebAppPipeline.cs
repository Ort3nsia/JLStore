using JLStore.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace JLStore.Infrastructure.Configuration;

public static class WebAppPipeline
{
    public static WebApplication UseAppDefaults(this WebApplication app)
    {
        if (app.Environment.IsDevelopment() ||
            Environment.GetEnvironmentVariable("FORCE_SWAGGER") == "1")
        {
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "JLStore API v1");
                o.RoutePrefix = string.Empty;
            });
        }

        if (!app.Environment.IsDevelopment())
            app.UseHttpsRedirection();

        app.MapControllers();
        return app;
    }

    public static async Task ApplyMigrationsAndSeedAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<DataContext>();

        if (app.Environment.IsDevelopment())
        {
            var attempts = 0;
            while (true)
            {
                try
                {
                    var pending = await db.Database.GetPendingMigrationsAsync();
                    if (pending.Any())
                        await db.Database.MigrateAsync();
                    break;
                }
                catch (PostgresException) when (attempts++ < 20) { await Task.Delay(TimeSpan.FromSeconds(3)); }
                catch (SqlException)      when (attempts++ < 20) { await Task.Delay(TimeSpan.FromSeconds(5)); }
            }
        }

        // Seed idempotente (deve poter girare più volte senza rompere)
        await DataSeed.EnsureSeedAsync(db);
    }

}
