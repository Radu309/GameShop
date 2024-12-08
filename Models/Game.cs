using System.ComponentModel.DataAnnotations;

namespace GameShop.Models;

public class Game
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Description { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int Stock { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public ICollection<Image> Images { get; set; } = new List<Image>();

    public ICollection<Review> Reviews { get; set; }  = new List<Review>();

}