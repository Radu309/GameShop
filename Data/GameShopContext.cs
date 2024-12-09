using GameShop.Models;
using Microsoft.EntityFrameworkCore;

namespace GameShop.Data;

public class GameShopContext : DbContext
{
    public GameShopContext(DbContextOptions<GameShopContext> options) : base(options) { }

    public DbSet<Game> Games { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Review> Reviews { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Game)
            .WithMany(g => g.OrderItems)
            .HasForeignKey(oi => oi.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Game)
            .WithMany(g => g.Reviews)
            .HasForeignKey(r => r.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.AppUser)
            .WithMany(c => c.Reviews)
            .HasForeignKey(r => r.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.AppUser)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Image>()
            .HasOne(i => i.Game)
            .WithMany(g => g.Images)
            .HasForeignKey(i => i.GameId)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
    }
}