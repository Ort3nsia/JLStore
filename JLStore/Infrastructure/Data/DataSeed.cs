using JLStore.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JLStore.Infrastructure.Data;

public static class DataSeed
{
    public static async Task EnsureSeedAsync(DataContext ctx)
    {
        await ctx.Database.MigrateAsync();

        if (!await ctx.Customers.AnyAsync())
        {
            ctx.Customers.AddRange(
                new Customer("Antonello", "Isabella"),
                new Customer("Andrea", "Leone")
            );
            await ctx.SaveChangesAsync();
        }
    }
}
