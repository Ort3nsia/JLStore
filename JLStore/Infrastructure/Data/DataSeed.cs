using JLStore.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JLStore.Infrastructure.Data;

public static class DataSeed
{
    public static async Task EnsureSeedAsync(DataContext ctx)
    {
        if (!await ctx.Customers.AnyAsync())
        {
            ctx.Customers.AddRange(
                new Customer("Antonello", "Isabella"),
                new Customer("Andrea", "Leone")
            );
            await ctx.SaveChangesAsync();
        }
        // Solo in Development: riallinea l'IDENTITY al valore massimo presente
        if (ctx.Database.IsSqlServer())
        {
            await ctx.Database.ExecuteSqlRawAsync(@"
                DECLARE @maxId INT = (SELECT ISNULL(MAX([ID]), 0) FROM [dbo].[Customers]);
                DBCC CHECKIDENT ('[dbo].[Customers]', RESEED, @maxId);
            ");
        }
    }
}
