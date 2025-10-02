
// Importa i modelli di dominio e le funzionalità di Entity Framework Core
using JLStore.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JLStore.Infrastructure.Data
{
    // DataContext rappresenta il contesto del database per Entity Framework Core.
    // Gestisce la configurazione delle entità e l'accesso ai dati.
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        // Configura la mappatura tra le classi del dominio e le tabelle del database.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurazione della tabella Customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Surname).IsRequired().HasMaxLength(50);
            });
        }

        public DbSet<Customer> Customers => Set<Customer>();
    }
}   