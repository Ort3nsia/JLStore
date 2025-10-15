using System;
using JLStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JLStore.Migrations.SqlServer
{
    public class SqlServerDesignTimeFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var cs = Environment.GetEnvironmentVariable("ConnectionStrings__SqlServer")
                ?? "Server=localhost,1433;Database=JLStore;User ID=sa;Password=Your_password123;TrustServerCertificate=True;";

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(cs, sql => sql.MigrationsAssembly(typeof(SqlServerDesignTimeFactory).Assembly.FullName))
                .Options;

            return new DataContext(options, TimeProvider.System);
        }
    }
}