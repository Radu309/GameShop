using GameShop.Models;
using Microsoft.EntityFrameworkCore;

namespace GameShop.Data;

public class GameShopContext : DbContext
{
    public GameShopContext(DbContextOptions<GameShopContext> options) : base(options) { }

    public DbSet<Game> Games { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Review> Reviews { get; set; }
}