using Microsoft.EntityFrameworkCore;
using UTechLeague24.Backend.Domain.Models;

namespace UTechLeague24.Backend.Domain.Db;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.UserRole)
                .IsRequired();

            entity.HasIndex(e => e.Username)
                .IsUnique();
        });
    }
}