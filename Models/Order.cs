namespace GameShop.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    // Relație - O comandă este făcută de un client
    public int ClientId { get; set; }
    public Client Client { get; set; }

    // Relație - O comandă poate conține mai multe produse
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
