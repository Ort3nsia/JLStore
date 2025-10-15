using System;
using JLStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JLStore.Migrations.Postgres
{
    public class PostgresDesignTimeFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var cs = Environment.GetEnvironmentVariable("ConnectionStrings__Default")
                ?? "Host=localhost;Port=5432;Database=JLStore;Username=postgres;Password=mysecretpassword;SSL Mode=Prefer;Trust Server Certificate=true";

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql(cs, npg => npg.MigrationsAssembly(typeof(PostgresDesignTimeFactory).Assembly.FullName))
                .Options;

            return new DataContext(options, TimeProvider.System);
        }
    }
}
