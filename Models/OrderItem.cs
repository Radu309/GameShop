using System.Text.Json.Serialization;

namespace GameShop.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    // Relație - Un OrderItem este asociat unui produs
    public int GameId { get; set; }
    [JsonIgnore] 
    public Game? Game { get; set; }

    // Relație - Un OrderItem este asociat unei comenzi
    public int OrderId { get; set; }
    [JsonIgnore] 
    public Order? Order { get; set; }
}