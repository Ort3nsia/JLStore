using JLStore.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DataContext>();

        if (app.Environment.IsDevelopment())
        {
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
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
            }
        }
        else
        {
            // In prod potresti voler migrare esplicitamente fuori dal runtime
        }

        await DataSeed.EnsureSeedAsync(db);
    }
}
