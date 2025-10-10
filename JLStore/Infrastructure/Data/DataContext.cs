// Importa i modelli di dominio e le funzionalità di Entity Framework Core
using JLStore.Domain.Models;
using JLStore.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace JLStore.Infrastructure.Data
{
    // Iniettiamo anche TimeProvider per avere "ora corrente" dinamica nei filtri globali
    public class DataContext(DbContextOptions<DataContext> options, TimeProvider timeProvider) : DbContext(options)
    {
        private readonly TimeProvider _timeProvider = timeProvider;

        // Proprietà esposta che EF parametrizza nei query filter
        public DateTimeOffset UtcNow => _timeProvider.GetUtcNow();

        // DbSet
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Post> Posts => Set<Post>();

        // Configura la mappatura tra le classi del dominio e le tabelle del database.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ---- Customer ----
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Surname).IsRequired().HasMaxLength(50);
            });

            // ---- Post ---- (config separata)
            modelBuilder.ApplyConfiguration(new PostConfiguration());

            // Filtro globale “pubblico” con orario dinamico (UTC)
            modelBuilder.Entity<Post>()
                .HasQueryFilter(p =>
                    !p.IsDeleted &&
                    p.Published &&
                    p.PublishedAt <= UtcNow);
        }
    }
}
