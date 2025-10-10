using JLStore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JLStore.Infrastructure.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> b)
        {
            b.ToTable("Posts");
            b.HasKey(p => p.Id);

            b.Property(p => p.Title).IsRequired().HasMaxLength(200);
            b.Property(p => p.Content).IsRequired();

            // Timestamp sempre in UTC lato DB
            b.Property(p => p.PublishedAt)
             .HasColumnType("datetimeoffset")
             .HasDefaultValueSql("SYSUTCDATETIME()");

            b.Property(p => p.Published).HasDefaultValue(false);
            b.Property(p => p.IsDeleted).HasDefaultValue(false);

            // Indici utili
            b.HasIndex(p => p.PublishedAt);
            b.HasIndex(p => new { p.Published, p.IsDeleted, p.PublishedAt });
        }
    }
}
