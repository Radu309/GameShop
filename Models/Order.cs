using System.Text.Json.Serialization;

namespace GameShop.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    [JsonIgnore] 
    public AppUser? AppUser { get; set; }
    public int AppUserId { get; set; }


    // Relație - O comandă poate conține mai multe produse
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
