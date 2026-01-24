using Microsoft.EntityFrameworkCore;
using nafsibooking.Models;

namespace nafsibooking.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Event> Events => Set<Event>();
    public DbSet<PromoterRequest> PromoterRequests => Set<PromoterRequest>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Event entity
        modelBuilder.Entity<Event>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Event>()
            .Property(e => e.Title)
            .IsRequired();

        // Store Highlights as JSON
        modelBuilder.Entity<Event>()
            .Property(e => e.Highlights)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',').ToList());

        // Store Tiers as JSON
        modelBuilder.Entity<Event>()
            .Property(e => e.Tiers)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<TicketTier>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<TicketTier>());

        // Configure PromoterRequest entity
        modelBuilder.Entity<PromoterRequest>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<PromoterRequest>()
            .Property(p => p.PromoterName)
            .IsRequired();

        modelBuilder.Entity<PromoterRequest>()
            .Property(p => p.PromoterEmail)
            .IsRequired();

        modelBuilder.Entity<PromoterRequest>()
            .Property(p => p.EventTitle)
            .IsRequired();

        // Configure User entity
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.DisplayName)
            .IsRequired();
    }
}
