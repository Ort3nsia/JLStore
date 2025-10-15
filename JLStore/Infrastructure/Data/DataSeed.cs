using JLStore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace JLStore.Infrastructure.Data;

public static class DataSeed
{
    public static async Task EnsureSeedAsync(DataContext ctx, CancellationToken ct = default)
    {
        var log = ctx.GetService<ILoggerFactory>().CreateLogger("DataSeed");

        var countBefore = await ctx.Customers.AsNoTracking().CountAsync(ct);
        log.LogInformation("Customers before seed: {Count}", countBefore);

        if (countBefore == 0)
        {
            ctx.Customers.AddRange(
                new Customer("Antonello", "Isabella"),
                new Customer("Andrea",    "Leone")
            );
            await ctx.SaveChangesAsync(ct);
            log.LogInformation("Inserted 2 Customers.");
        }
        else
        {
            log.LogInformation("Skipping insert: Customers already present.");
        }

        // Non far fallire il seed per un problema di reseed: logga e continua
        try
        {
            if (ctx.Database.IsSqlServer())
            {
                await ctx.Database.ExecuteSqlRawAsync(@"
                    DECLARE @maxId INT = (SELECT ISNULL(MAX([ID]), 0) FROM [dbo].[Customers]);
                    DBCC CHECKIDENT ('[dbo].[Customers]', RESEED, @maxId);
                ", ct);
            }
            else if (ctx.Database.IsNpgsql())
            {
                await ctx.Database.ExecuteSqlRawAsync(@"
                    SELECT setval(
                        pg_get_serial_sequence('""Customers""','ID'),
                        COALESCE((SELECT MAX(""ID"") FROM ""Customers""), 0)
                    );
                ", ct);
            }
            log.LogInformation("Reseed completed.");
        }
        catch (Exception ex)
        {
            log.LogWarning(ex, "Reseed skipped due to error.");
        }

        var countAfter = await ctx.Customers.AsNoTracking().CountAsync(ct);
        log.LogInformation("Customers after seed: {Count}", countAfter);
    }
}
